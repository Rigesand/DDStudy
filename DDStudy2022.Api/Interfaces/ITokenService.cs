using DDStudy2022.Api.Models.Tokens;

namespace DDStudy2022.Api.Interfaces;

public interface ITokenService
{
    public Task<TokenModel> Login(string login, string password);
    public Task<TokenModel> GetTokenByRefreshToken(string refreshToken);
}