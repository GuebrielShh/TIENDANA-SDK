using TiendanaMP.SDK.Models;
using TiendanaMP.SDK.Services.Implementations;
using TiendanaMP.SDK.Services.Interfaces;
namespace TiendanaMP.SDK.Services;


/// <summary>
/// Servicio para interactuar con los métodos de pago de MercadoPago.
/// </summary>
public class MercadoPagoPaymentMethodService
{
    private readonly ITokenStorageService _tokenStorage;
    private readonly MercadoPagoOAuthService _oauthService;
    private readonly IConfiguration _config;

    /// <summary>
    /// Constructor que recibe las dependencias necesarias.
    /// </summary>
    public MercadoPagoPaymentMethodService(ITokenStorageService tokenStorage, MercadoPagoOAuthService oauthService, IConfiguration config)
    {
        _tokenStorage = tokenStorage;
        _oauthService = oauthService;
        _config = config;
    }

    /// <summary>
    /// Valida que la configuración de MercadoPago esté presente.
    /// </summary>
    private void ValidateConfig()
    {
        if (string.IsNullOrEmpty(_config["MercadoPago:ClientId"]) ||
            string.IsNullOrEmpty(_config["MercadoPago:ClientSecret"]))
            throw new InvalidOperationException("ClientId o ClientSecret de MercadoPago no están configurados.");
    }

    /// <summary>
    /// Obtiene un access token válido para el usuario, refrescándolo si es necesario.
    /// </summary>
    private async Task<string> GetValidAccessTokenAsync(string userId)
    {
        // Obtiene los tokens almacenados para el usuario
        var tokens = await _tokenStorage.GetTokensAsync(userId)
            ?? throw new UnauthorizedAccessException("No se encontraron tokens para el usuario.");

        // Verifica que el access token no sea nulo
        if (tokens.Access_token == null)
            throw new UnauthorizedAccessException("El access token es nulo.");

        // Calcula el tiempo de expiración del token
        var expirationTime = DateTime.UtcNow.AddSeconds(tokens.Expires_in);
        // Si el token aún es válido por más de 5 minutos, lo retorna
        if (expirationTime > DateTime.UtcNow.AddMinutes(5))
            return tokens.Access_token;

        // Valida la configuración antes de refrescar el token
        ValidateConfig();

        // Verifica que el refresh token no sea nulo
        if (string.IsNullOrEmpty(tokens.Refresh_token))
            throw new UnauthorizedAccessException("El refresh token es nulo.");

        var clientId = _config["MercadoPago:ClientId"];
        if (string.IsNullOrEmpty(clientId))
            throw new InvalidOperationException("ClientId no está configurado.");

        var clientSecret = _config["MercadoPago:ClientSecret"];
        if (string.IsNullOrEmpty(clientSecret))
            throw new InvalidOperationException("ClientSecret no está configurado.");

        // Refresca el access token usando el refresh token
        var newTokens = await _oauthService.RefreshAccessTokenAsync(clientId, clientSecret, tokens.Refresh_token)
            ?? throw new UnauthorizedAccessException("No se pudo refrescar el token de acceso (posiblemente nulo).");

        if (string.IsNullOrEmpty(newTokens.Access_token))
            throw new UnauthorizedAccessException("No se pudo refrescar el token de acceso.");

        // Actualiza los tokens almacenados
        await _tokenStorage.UpdateTokensAsync(userId, newTokens);
        return newTokens.Access_token;
    }

    /// <summary>
    /// Obtiene la lista de métodos de pago disponibles para el usuario.
    /// </summary>
    public async Task<List<PaymentMethod>> GetPaymentMethodsAsync(string userId)
    {
        // Obtiene un access token válido
        var accessToken = await GetValidAccessTokenAsync(userId);

        // Crea el cliente y la solicitud HTTP
        var client = new RestClient(MercadoPagoConstants.BASE_URL);
        var request = new RestRequest(MercadoPagoConstants.PAYMENT_METHODS_ENDPOINT, Method.Get)
            .AddHeader("Authorization", $"Bearer {accessToken}")
            .AddHeader("Accept", "application/json");

        // Ejecuta la solicitud
        var response = await client.ExecuteAsync(request);

        // Si la respuesta no es exitosa, lanza una excepción
        if (!response.IsSuccessful)
            throw new Exception($"Error obteniendo métodos de pago: {response.Content}");

        // Deserializa la respuesta o retorna una lista vacía
        return string.IsNullOrEmpty(response.Content)
            ? new List<PaymentMethod>()
            : JsonSerializer.Deserialize<List<PaymentMethod>>(response.Content) ?? new List<PaymentMethod>();
    }

    /// <summary>
    /// Obtiene la lista de bancos PSE disponibles para el usuario.
    /// </summary>
    public async Task<List<FinancialInstitution>> GetPseBanksAsync(string userId)
    {
        // Obtiene un access token válido
        var accessToken = await GetValidAccessTokenAsync(userId);

        // Crea el cliente y la solicitud HTTP
        var client = new RestClient(MercadoPagoConstants.BASE_URL);
        var request = new RestRequest(MercadoPagoConstants.FINANCIAL_INSTITUTIONS_ENDPOINT, Method.Get)
            .AddHeader("Authorization", $"Bearer {accessToken}")
            .AddHeader("Accept", "application/json")
            .AddParameter("payment_method_id", "pse", ParameterType.QueryString);

        // Ejecuta la solicitud
        var response = await client.ExecuteAsync(request);

        // Si la respuesta no es exitosa, lanza una excepción
        if (!response.IsSuccessful)
            throw new Exception($"Error obteniendo bancos PSE: {response.Content}");

        // Deserializa la respuesta o retorna una lista vacía
        return string.IsNullOrEmpty(response.Content)
            ? new List<FinancialInstitution>()
            : JsonSerializer.Deserialize<List<FinancialInstitution>>(response.Content) ?? new List<FinancialInstitution>();
    }
}
