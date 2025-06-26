using TiendanaMP.SDK.Models.Response;
using TiendanaMP.SDK.Services.Interfaces;

namespace TiendanaMP.SDK.Services.Interfaces
{
    public interface ITokenStorageService
    {
        Task SaveTokensAsync(string userId, TokenResponse tokens);
        Task<TokenResponse?> GetTokensAsync(string userId);
        Task UpdateTokensAsync(string userId, TokenResponse tokens);
    }
}

// Servicio en memoria para guardar y recuperar tokens de usuario.
public class InMemoryTokenStorageService : ITokenStorageService
{
    private readonly Dictionary<string, TokenResponse> _store = new();

    public Task SaveTokensAsync(string userId, TokenResponse tokens)
    {
        _store[userId] = tokens;
        return Task.CompletedTask;
    }

    public Task<TokenResponse?> GetTokensAsync(string userId)
    {
        _store.TryGetValue(userId, out var tokens);
        return Task.FromResult(tokens);
    }

    public Task UpdateTokensAsync(string userId, TokenResponse tokens)
    {
        _store[userId] = tokens;
        return Task.CompletedTask;
    }
}
