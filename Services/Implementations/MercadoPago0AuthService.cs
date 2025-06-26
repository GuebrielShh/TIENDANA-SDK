using TiendanaMP.SDK.Models.Response;
using TiendanaMP.SDK.Services.Interfaces;

namespace TiendanaMP.SDK.Services.Implementations;
/// <summary>
/// Servicio para manejar la autenticación OAuth con MercadoPago.
/// </summary>
public class MercadoPagoOAuthService
{
    // Cliente HTTP para realizar peticiones a la API de MercadoPago.
    private readonly RestClient _client;
    // Configuración de la aplicación (por ejemplo, ClientId y ClientSecret).
    private readonly IConfiguration _config;
    // Servicio para almacenar y recuperar tokens.
    private readonly ITokenStorageService _tokenStorage;
    // Diccionario concurrente para llevar registro de cuándo se obtuvo el token por usuario.
    private readonly ConcurrentDictionary<string, DateTime> _tokenObtainedAt = new();

    /// <summary>
    /// Constructor que inicializa las dependencias necesarias.
    /// </summary>
    public MercadoPagoOAuthService(IConfiguration config, ITokenStorageService tokenStorage)
    {
        _config = config;
        _tokenStorage = tokenStorage;
        // Inicializa el cliente RestSharp con la URL base para obtener tokens.
        _client = new RestClient($"{MercadoPagoConstants.BASE_URL}{MercadoPagoConstants.OAUTH_TOKEN_ENDPOINT}");
    }

    /// <summary>
    /// Genera la URL de autorización para iniciar el flujo OAuth.
    /// </summary>
    public string GetAuthorizationUrl(string clientId, string redirectUri, string state, string codeChallenge)
    {
        return $"{MercadoPagoConstants.AUTH_BASE_URL}{MercadoPagoConstants.OAUTH_AUTHORIZE_ENDPOINT}" +
               $"?client_id={clientId}" +
               $"&response_type=code" +
               $"&platform_id=mp" +
               $"&state={state}" +
               $"&redirect_uri={Uri.EscapeDataString(redirectUri)}" +
               $"&code_challenge={codeChallenge}" +
               $"&code_challenge_method=S256";
    }

    /// <summary>
    /// Intercambia el código de autorización por un token de acceso y refresh.
    /// </summary>
    public async Task<TokenResponse> ExchangeCodeForTokenAsync(string clientId, string clientSecret, string code, string redirectUri, string codeVerifier)
    {
        var request = new RestRequest
        {
            Method = Method.Post
        };
        request.AddHeader("Content-Type", "application/json");
        var body = new
        {
            client_id = clientId,
            client_secret = clientSecret,
            grant_type = "authorization_code",
            code,
            redirect_uri = redirectUri,
            code_verifier = codeVerifier
        };
        request.AddStringBody(JsonSerializer.Serialize(body), DataFormat.Json);

        var response = await _client.ExecuteAsync(request);
        if (!response.IsSuccessful)
            throw new Exception($"Error al obtener token: {response.Content}");
        if (string.IsNullOrEmpty(response.Content))
            throw new Exception("La respuesta del servidor está vacía.");

        return JsonSerializer.Deserialize<TokenResponse>(response.Content)
               ?? throw new Exception("No se pudo deserializar la respuesta del token.");
    }

    /// <summary>
    /// Usa el refresh token para obtener un nuevo access token.
    /// </summary>
    public async Task<TokenResponse> RefreshAccessTokenAsync(string clientId, string clientSecret, string refreshToken)
    {
        var request = new RestRequest
        {
            Method = Method.Post
        };

        request.AddHeader("Content-Type", "application/json");
        var body = new
        {
            client_id = clientId,
            client_secret = clientSecret,
            grant_type = "refresh_token",
            refresh_token = refreshToken
        };
        request.AddStringBody(JsonSerializer.Serialize(body), DataFormat.Json);

        var response = await _client.ExecuteAsync(request);
        if (!response.IsSuccessful)
            throw new Exception($"Error al renovar token: {response.Content}");
        if (string.IsNullOrEmpty(response.Content))
            throw new Exception("La respuesta del servidor está vacía.");

        return JsonSerializer.Deserialize<TokenResponse>(response.Content)
               ?? throw new Exception("No se pudo deserializar la respuesta del token.");
    }

    /// <summary>
    /// Obtiene un access token válido para el usuario, renovándolo si es necesario.
    /// </summary>
    public async Task<string> GetValidAccessTokenAsync(string userId)
    {
        // Recupera los tokens almacenados para el usuario.
        var tokens = await _tokenStorage.GetTokensAsync(userId)
                      ?? throw new UnauthorizedAccessException("No se encontraron tokens para el usuario.");

        if (string.IsNullOrEmpty(tokens.Refresh_token))
            throw new UnauthorizedAccessException("El refresh token es nulo o vacío.");

        // Obtiene la fecha en que se obtuvo el token.
        _tokenObtainedAt.TryGetValue(userId, out var obtainedAt);
        var expirationTime = obtainedAt.AddSeconds(tokens.Expires_in);

        // Si el token está por expirar (menos de 5 minutos), lo renueva.
        if (DateTime.UtcNow >= expirationTime.AddMinutes(-5))
        {
            var clientId = _config["MercadoPago:ClientId"]
                ?? throw new InvalidOperationException("ClientId no está configurado.");

            var clientSecret = _config["MercadoPago:ClientSecret"]
                ?? throw new InvalidOperationException("ClientSecret no está configurado.");

            var newTokens = await RefreshAccessTokenAsync(clientId, clientSecret, tokens.Refresh_token);
            if (string.IsNullOrEmpty(newTokens.Access_token))
                throw new UnauthorizedAccessException("No se pudo renovar el token de acceso.");

            await _tokenStorage.UpdateTokensAsync(userId, newTokens);
            _tokenObtainedAt[userId] = DateTime.UtcNow;

            return newTokens.Access_token;
        }

        // Si el token aún es válido, lo retorna.
        return tokens.Access_token
               ?? throw new UnauthorizedAccessException("El access token es nulo.");
    }

    /// <summary>
    /// Permite establecer manualmente la fecha en que se obtuvo el token para un usuario.
    /// </summary>
    public void SetTokenObtainedAt(string userId, DateTime dateTime)
    {
        _tokenObtainedAt[userId] = dateTime;
    }

    internal TokenResponse RefreshAccessToken(string clientId, string clientSecret, object refresh_token)
    {
        throw new NotImplementedException();
    }
}
