using DDStudy2022.Api.Models.Attaches;
using DDStudy2022.Api.Models.Users;

namespace DDStudy2022.Api.Interfaces;

public interface IUserService
{
    public Task<bool> CheckUserExist(string email);
    public Task AddAvatarToUser(Guid userId, MetadataModel meta, string filePath);
    public Task<AttachModel> GetUserAvatar(Guid userId);
    public Task<IEnumerable<UserAvatarModel>> GetUsers();
    public Task<UserAvatarModel> GetUser(Guid id);
}