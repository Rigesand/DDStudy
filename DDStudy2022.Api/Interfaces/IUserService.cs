using DDStudy2022.Api.Models;

namespace DDStudy2022.Api.Interfaces;

public interface IUserService
{
    public Task CreateUser(CreateUserModel model);
    public Task<IEnumerable<UserModel>> GetAllUsers();
}