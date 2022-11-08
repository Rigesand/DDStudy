using DDStudy2022.Api.Models.Attaches;

namespace DDStudy2022.Api.Interfaces;

public interface IAttachService
{
    public Task<MetadataModel> UploadFile(IFormFile file);
    public string GetPath(MetadataModel model);
}