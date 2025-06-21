using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using RestSharp;
using System.Text.Json;
using TiendanaMP.SDK.Models;

using TiendanaMP.SDK.Models.Response;
using TiendanaMP.SDK.Services.Implementations;
using TiendanaMP.SDK.Models.Token_Request;
using TiendanaMP.SDK.Services.Interfaces;

namespace TiendanaMP.SDK.Services;

/// <summary>
/// Servicio para gestionar pagos con MercadoPago.
/// </summary>
public class MercadoPagoPaymentService
{
    private readonly ITokenStorageService _tokenStorage;
    private readonly MercadoPagoOAuthService _oauthService;
    private readonly IConfiguration _config;

    public MercadoPagoPaymentService(ITokenStorageService tokenStorage, MercadoPagoOAuthService oauthService, IConfiguration config)
    {
        _tokenStorage = tokenStorage;
        _oauthService = oauthService;
        _config = config;
    }

    private async Task<string> GetValidAccessTokenAsync(string userId)
    {
        var tokens = await _tokenStorage.GetTokensAsync(userId);
        if (tokens == null || string.IsNullOrEmpty(tokens.Refresh_token))
            throw new UnauthorizedAccessException("No se encontraron tokens o el refresh token es inválido.");

        var expirationTime = tokens.ObtainedAt.AddSeconds(tokens.Expires_in);

        if (expirationTime <= DateTime.UtcNow.AddMinutes(5))
        {
            var clientId = _config["MercadoPago:ClientId"];
            var clientSecret = _config["MercadoPago:ClientSecret"];

            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
                throw new InvalidOperationException("ClientId o ClientSecret no configurados.");

            var newTokens = await _oauthService.RefreshAccessTokenAsync(clientId, clientSecret, tokens.Refresh_token);
            if (newTokens == null || string.IsNullOrEmpty(newTokens.Access_token))
                throw new UnauthorizedAccessException("No se pudo renovar el token de acceso.");

            await _tokenStorage.UpdateTokensAsync(userId, newTokens);
            tokens = newTokens;
        }

        if (string.IsNullOrEmpty(tokens.Access_token))
            throw new UnauthorizedAccessException("El access token es nulo o vacío.");

        return tokens.Access_token
               ?? throw new UnauthorizedAccessException("El access token es nulo.");
    }

    public async Task<PaymentResponse> CreateCardPaymentAsync(string userId, CardPaymentRequest request)
    {
        var accessToken = await GetValidAccessTokenAsync(userId);

        var client = new RestClient(MercadoPagoConstants.BASE_URL);
        var restRequest = new RestRequest(MercadoPagoConstants.PAYMENT_ENDPOINT, Method.Post);

        restRequest.AddHeader("Authorization", $"Bearer {accessToken}");
        restRequest.AddHeader("Content-Type", "application/json");
        restRequest.AddHeader("X-Idempotency-Key", Guid.NewGuid().ToString());

        var body = new
        {
            transaction_amount = request.TransactionAmount,
            token = request.Token,
            description = request.Description,
            installments = request.Installments,
            payment_method_id = request.PaymentMethodId,
            issuer_id = request.IssuerId,
            payer = new
            {
                email = request.PayerEmail,
                identification = new
                {
                    type = request.CardholderIdentificationType,
                    number = request.CardholderIdentificationNumber
                },
                first_name = request.CardholderName?.Split(" ").FirstOrDefault(),
                last_name = request.CardholderName?.Split(" ").Skip(1).FirstOrDefault()
            },
            notification_url = request.NotificationUrl,
            additional_info = new
            {
                ip_address = request.IpAddress
            }
        };

        restRequest.AddJsonBody(body);
        var response = await client.ExecuteAsync(restRequest);

        if (!response.IsSuccessful || string.IsNullOrEmpty(response.Content))
            throw new Exception($"Error creando pago con tarjeta: {response.Content}");

        return JsonSerializer.Deserialize<PaymentResponse>(response.Content)
               ?? throw new Exception("No se pudo deserializar la respuesta de pago.");
    }

    public async Task<PaymentResponse> CreatePsePaymentAsync(string userId, PsePaymentRequest request, HttpContext httpContext)
    {
        var accessToken = await GetValidAccessTokenAsync(userId);

        var client = new RestClient(MercadoPagoConstants.BASE_URL);
        var restRequest = new RestRequest(MercadoPagoConstants.PAYMENT_ENDPOINT, Method.Post);

        restRequest.AddHeader("Authorization", $"Bearer {accessToken}");
        restRequest.AddHeader("Content-Type", "application/json");
        restRequest.AddHeader("X-Idempotency-Key", Guid.NewGuid().ToString());

        var body = new
        {
            transaction_amount = request.TransactionAmount,
            description = request.Description,
            payment_method_id = request.PaymentMethodId,
            payer = new
            {
                email = request.Payer.Email,
                first_name = request.Payer.FirstName,
                last_name = request.Payer.LastName,
                entity_type = request.Payer.EntityType,
                type = request.Payer.Type,
                identification = new
                {
                    type = request.Payer.Identification.Type,
                    number = request.Payer.Identification.Number
                }
            },
            issuer_id = request.Payer.AdditionalInfo.FinancialInstitution,
            callback_url = request.CallbackUrl,
            notification_url = request.NotificationUrl,
            additional_info = new
            {
                ip_address = request.Payer.AdditionalInfo.IpAddress
            }
        };

        restRequest.AddJsonBody(body);
        var response = await client.ExecuteAsync(restRequest);

        if (!response.IsSuccessful || string.IsNullOrEmpty(response.Content))
            throw new Exception($"Error creando pago PSE: {response.Content}");

        return JsonSerializer.Deserialize<PaymentResponse>(response.Content)
               ?? throw new Exception("No se pudo deserializar la respuesta de pago.");
    }
}
