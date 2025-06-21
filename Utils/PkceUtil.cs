using System.Security.Cryptography;
using System.Text;

/// <summary>
/// Utilidad para generar los parámetros PKCE (Proof Key for Code Exchange) usados en flujos de autenticación OAuth 2.0.
/// </summary>
public static class PkceUtil
{
    /// <summary>
    /// Genera un *code verifier*, una cadena aleatoria segura, con los caracteres permitidos por el estándar PKCE.
    /// </summary>
    /// <param name="length">Longitud del code verifier. Por defecto, 64 caracteres.</param>
    /// <returns>Una cadena aleatoria segura.</returns>
    public static string GenerateCodeVerifier(int length = 64)
    {
        // Caracteres válidos según el estándar para el code_verifier
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._~";

        // Generador criptográfico de números aleatorios
        var random = RandomNumberGenerator.Create();
        var bytes = new byte[length];
        random.GetBytes(bytes); // Llena el arreglo con bytes aleatorios

        var charsArray = chars.ToCharArray();
        var result = new StringBuilder(length);

        // Convierte cada byte en un carácter permitido del conjunto
        foreach (var b in bytes)
        {
            result.Append(charsArray[b % charsArray.Length]);
        }

        return result.ToString();
    }

    /// <summary>
    /// Genera el *code challenge* a partir de un code verifier, usando SHA256 y codificación Base64 URL-safe.
    /// </summary>
    /// <param name="codeVerifier">El code verifier previamente generado.</param>
    /// <returns>El code challenge codificado.</returns>
    public static string GenerateCodeChallenge(string codeVerifier)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.ASCII.GetBytes(codeVerifier)); // Hash SHA-256 del code verifier
        return Base64UrlEncode(bytes); // Codifica el hash en Base64 URL-safe
    }

    /// <summary>
    /// Codifica un arreglo de bytes a Base64 con formato URL-safe (sin padding ni caracteres conflictivos).
    /// </summary>
    /// <param name="input">Bytes a codificar.</param>
    /// <returns>Cadena codificada en Base64 URL-safe.</returns>
    private static string Base64UrlEncode(byte[] input)
    {
        return Convert.ToBase64String(input)
            .Replace("+", "-") // Reemplaza caracteres no válidos en URLs
            .Replace("/", "_")
            .Replace("=", ""); // Elimina el padding
    }
}
