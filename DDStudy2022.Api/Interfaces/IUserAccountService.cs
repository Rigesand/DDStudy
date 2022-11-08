using DDStudy2022.Api.Models.Attaches;

namespace DDStudy2022.Api.Interfaces;

public interface IUserAccountService
{
    public Task AddAvatarToUser(Guid userId, MetadataModel meta, string filePath);
    public Task<AttachModel> GetUserAvatar(Guid userId);
}