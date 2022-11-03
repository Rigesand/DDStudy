using DDStudy2022.Api.Models.Tokens;
using DDStudy2022.Api.Models.Users;

namespace DDStudy2022.Api.Interfaces;

public interface ITokenService
{
    public Task<TokenModel> Login(string login, string password);
    public Task Registration(CreateUserModel user);
    public Task<TokenModel> GetTokenByRefreshToken(string refreshToken);
}