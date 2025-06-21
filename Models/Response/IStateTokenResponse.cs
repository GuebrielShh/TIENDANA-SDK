namespace TiendanaMP.SDK.Utils
{
    /// <summary>
    /// Define la interfaz para el servicio de manejo del parámetro "state" en flujos OAuth 2.0.
    /// Este servicio ayuda a prevenir ataques CSRF y enlaza el código de verificación (PKCE) con el estado.
    /// </summary>
    public interface IStateTokenService
    {
        /// <summary>
        /// Genera un valor "state" firmado digitalmente, que se enviará a la URL de autorización OAuth.
        /// </summary>
        /// <returns>Cadena de estado firmada.</returns>
        string GenerateSignedState();

        /// <summary>
        /// Valida si el valor del estado recibido es auténtico y no ha sido modificado.
        /// </summary>
        /// <param name="state">Valor de estado recibido en el callback OAuth.</param>
        /// <returns>True si es válido, false si es inválido o comprometido.</returns>
        bool ValidateSignedState(string state);

        /// <summary>
        /// Almacena temporalmente el "state" y su correspondiente "code_verifier" generado durante el flujo PKCE.
        /// </summary>
        /// <param name="state">Valor de estado generado.</param>
        /// <param name="codeVerifier">Código de verificación asociado al flujo PKCE.</param>
        void Save(string state, string codeVerifier);

        /// <summary>
        /// Obtiene el "code_verifier" almacenado previamente para un valor de estado dado.
        /// </summary>
        /// <param name="state">Estado utilizado para recuperar el verificador.</param>
        /// <returns>El código de verificación si existe, null si no se encuentra.</returns>
        string? Get(string state);
    }
}
