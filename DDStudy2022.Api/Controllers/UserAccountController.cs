using DDStudy2022.Api.Interfaces;
using DDStudy2022.Api.Models.Attaches;
using DDStudy2022.Common.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DDStudy2022.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class UserAccountController : ControllerBase
{
    private readonly IUserAccountService _userAccountService;

    public UserAccountController(IUserAccountService userAccountService)
    {
        _userAccountService = userAccountService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task AddAvatarToUser(MetadataModel model)
    {
        var userIdString = User.Claims.FirstOrDefault(x => x.Type == "userAccountId")?.Value;
        if (Guid.TryParse(userIdString, out var userId))
        {
            var tempFi = new FileInfo(Path.Combine(Path.GetTempPath(), model.TempId.ToString()));
            if (!tempFi.Exists)
                throw new FileException("file not found");

            var path = Path.Combine(Directory.GetCurrentDirectory(), "Attaches", model.TempId.ToString());
            var destFi = new FileInfo(path);
            if (destFi.Directory != null && !destFi.Directory.Exists)
                destFi.Directory.Create();

            System.IO.File.Copy(tempFi.FullName, path, true);

            await _userAccountService.AddAvatarToUser(userId, model, path);
        }
        else
            throw new UserException("You are not authorized");
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<FileResult> GetUserAvatar(Guid userAccountId)
    {
        var attach = await _userAccountService.GetUserAvatar(userAccountId);

        return File(System.IO.File.ReadAllBytes(attach.FilePath), attach.MimeType);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<FileResult> DownloadAvatar(Guid userAccountId)
    {
        var attach = await _userAccountService.GetUserAvatar(userAccountId);

        HttpContext.Response.ContentType = attach.MimeType;
        FileContentResult result = new FileContentResult(System.IO.File.ReadAllBytes(attach.FilePath), attach.MimeType)
        {
            FileDownloadName = attach.Name
        };

        return result;
    }
}