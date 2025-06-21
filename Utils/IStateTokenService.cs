using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using TiendanaMP.SDK.Utils;

namespace MercadoPagoAPI.Utils
{
    // Servicio para generar y validar tokens de estado firmados (state tokens)
    public class StateTokenService : IStateTokenService
    {
        // Diccionario en memoria para almacenar code verifiers asociados a un state
        // En producción, se recomienda usar una base de datos u otro almacenamiento persistente
        private static readonly Dictionary<string, string> _verifiers = new();

        // Clave secreta para firmar los tokens
        private readonly byte[] _secretKey;

        // Constructor que recibe la configuración y obtiene la clave secreta
        public StateTokenService(IConfiguration config)
        {
            var key = config["MercadoPago:StateSigningKey"];
            if (string.IsNullOrEmpty(key))
                throw new Exception("Falta configurar MercadoPago:StateSigningKey");

            _secretKey = Encoding.UTF8.GetBytes(key);
        }

        // Genera un nuevo state firmado (state.token)
        public string GenerateSignedState()
        {
            var rawState = Guid.NewGuid().ToString(); // Genera un identificador único
            var signature = ComputeHmac(rawState);    // Calcula la firma HMAC
            return $"{rawState}.{signature}";         // Une el state y la firma
        }

        // Valida que el state recibido tenga una firma válida
        public bool ValidateSignedState(string state)
        {
            var parts = state.Split('.');
            if (parts.Length != 2) return false; // Debe tener dos partes: state y firma

            var rawState = parts[0];
            var receivedSignature = parts[1];
            var expectedSignature = ComputeHmac(rawState);

            return receivedSignature == expectedSignature; // Compara la firma esperada con la recibida
        }

        // Guarda el code verifier asociado a un state
        public void Save(string state, string codeVerifier)
        {
            _verifiers[state] = codeVerifier;
        }

        // Recupera el code verifier asociado a un state, si existe
        public string? Get(string state)
        {
            return _verifiers.TryGetValue(state, out var verifier) ? verifier : null;
        }

        // Calcula la firma HMAC-SHA256 de un string usando la clave secreta
        private string ComputeHmac(string data)
        {
            using var hmac = new HMACSHA256(_secretKey);
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            // Codifica en Base64 URL-safe (sin =, +, /)
            return Convert.ToBase64String(hash)
                .Replace("=", "").Replace("+", "-").Replace("/", "_");
        }
    }
}
