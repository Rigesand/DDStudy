using DDStudy2022.Api.Models.Users;
using DDStudy2022.DAL.Entities;

namespace DDStudy2022.Api.Interfaces;

public interface IUserService
{
    public Task UpdateUser(UpdateUser user);
    public Task DeleteUser(Guid id);
    public Task<bool> CheckUserExistsByMail(string email);
    public Task<UserModel> GetUser(Guid id);
    public Task<IEnumerable<UserModel>> GetAllUsers();
    public Task<UserSession> GetSessionById(Guid id);
}