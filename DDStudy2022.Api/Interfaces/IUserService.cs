using DDStudy2022.Api.Models.Users;

namespace DDStudy2022.Api.Interfaces;

public interface IUserService
{
    public Task CreateUser(CreateUserModel user);
    public Task UpdateUser(UpdateUser user);
    public Task DeleteUser(Guid id);
    public Task<bool> FindByMail(string email);
    public Task<UserModel> GetUser(Guid id);
    public Task<IEnumerable<UserModel>> GetAllUsers();
}