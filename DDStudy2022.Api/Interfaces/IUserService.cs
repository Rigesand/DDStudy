using DDStudy2022.Api.Models;

namespace DDStudy2022.Api.Interfaces;

public interface IUserService
{
    public Task CreateUser(CreateUserModel user);
    public Task<IEnumerable<UserModel>> GetAllUsers();
    public Task<bool> FindByMail(CreateUserModel user);
}