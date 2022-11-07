using DDStudy2022.Api.Interfaces;
using DDStudy2022.Api.Models.Attaches;
using DDStudy2022.Api.Models.Users;
using DDStudy2022.Common.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DDStudy2022.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task DeleteUser(Guid id)
    {
        await _userService.DeleteUser(id);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserModel>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IEnumerable<UserModel>> GetAllUsers()
    {
        return await _userService.GetAllUsers();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<UserModel> GetCurrentUser()
    {
        var userIdString = User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
        if (Guid.TryParse(userIdString, out var userId))
        {
            return await _userService.GetUser(userId);
        }

        throw new UserException("You are not authorized");
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task AddAvatarToUser(MetadataModel model)
    {
        var userIdString = User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
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

            await _userService.AddAvatarToUser(userId, model, path);
        }
        else
            throw new UserException("You are not authorized");
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<FileResult> GetUserAvatar(Guid userId)
    {
        var attach = await _userService.GetUserAvatar(userId);

        return File(System.IO.File.ReadAllBytes(attach.FilePath!), attach.MimeType!);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<FileResult> DownloadAvatar(Guid userId)
    {
        var attach = await _userService.GetUserAvatar(userId);

        HttpContext.Response.ContentType = attach.MimeType!;
        FileContentResult result =
            new FileContentResult(System.IO.File.ReadAllBytes(attach.FilePath!), attach.MimeType!)
            {
                FileDownloadName = attach.Name
            };

        return result;
    }
}