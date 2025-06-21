namespace TiendanaMP.SDK.Utils
{
    public interface IStateTokenService
    {
        string GenerateSignedState();
        bool ValidateSignedState(string state);
        void Save(string state, string codeVerifier);
        string? Get(string state);
    }
}
