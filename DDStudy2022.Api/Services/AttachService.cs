using DDStudy2022.Api.Interfaces;
using DDStudy2022.Api.Models.Attaches;
using DDStudy2022.Common.Exceptions;

namespace DDStudy2022.Api.Services;

public class AttachService : IAttachService
{
    public async Task<MetadataModel> UploadFile(IFormFile file)
    {
        var tempPath = Path.GetTempPath();
        var meta = new MetadataModel
        {
            TempId = Guid.NewGuid(),
            Name = file.FileName,
            MimeType = file.ContentType,
            Size = file.Length
        };
        var newPath = Path.Combine(tempPath, meta.TempId.ToString());

        var fileInfo = new FileInfo(newPath);

        if (fileInfo.Exists)
        {
            throw new FileException("File already exists");
        }

        if (fileInfo.Directory == null)
        {
            throw new FileException("Temp is null");
        }

        if (!fileInfo.Directory.Exists)
        {
            fileInfo.Directory?.Create();
        }

        using (var stream = File.Create(newPath))
        {
            await file.CopyToAsync(stream);
        }

        return meta;
    }
}