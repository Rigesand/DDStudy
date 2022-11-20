using DDStudy2022.Api.Interfaces;
using DDStudy2022.Api.Models.Attaches;
using DDStudy2022.Api.Models.Users;
using DDStudy2022.Api.Services;
using DDStudy2022.Common.Consts;
using DDStudy2022.Common.Exceptions;
using DDStudy2022.Common.Exstensions;
using Microsoft.AspNetCore.Mvc;

namespace DDStudy2022.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[ApiExplorerSettings(GroupName = "Api")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAttachService _attachService;

    public UserController(IUserService userService, LinkGeneratorService links, IAttachService attachService)
    {
        _userService = userService;
        _attachService = attachService;

        links.LinkAvatarGenerator = x =>
            Url.ControllerAction<AttachController>(nameof(AttachController.GetUserAvatar), new
            {
                userId = x.Id,
            });
    }

    [HttpPost]
    public async Task AddAvatarToUser(MetadataModel model)
    {
        var userId = User.GetClaimValue<Guid>(ClaimNames.Id);
        if (userId != default)
        {
            var path = _attachService.GetPath(model);
            await _userService.AddAvatarToUser(userId, model, path);
        }
        else
            throw new UserException("you are not authorized");
    }

    [HttpGet]
    public async Task<IEnumerable<UserAvatarModel>> GetUsers() => await _userService.GetUsers();

    [HttpGet]
    public async Task<UserAvatarModel> GetCurrentUser()
    {
        var userId = User.GetClaimValue<Guid>(ClaimNames.Id);
        if (userId != default)
        {
            return await _userService.GetUser(userId);
        }

        throw new UserException("you are not authorized");
    }

    [HttpGet]
    public async Task<UserAvatarModel> GetUserById(Guid userId)
    {
        return await _userService.GetUser(userId);
    }
}