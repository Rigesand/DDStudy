using DDStudy2022.Api.Models.Tokens;
using DDStudy2022.Api.Models.Users;
using DDStudy2022.DAL.Entities;

namespace DDStudy2022.Api.Interfaces;

public interface IAuthService
{
    public Task<TokenModel> Login(string login, string password);
    public Task Registration(CreateUserModel user);
    public Task<TokenModel> GetTokenByRefreshToken(string refreshToken);
    public Task<UserSession> GetSessionById(Guid id);
}