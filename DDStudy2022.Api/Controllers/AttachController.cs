using DDStudy2022.Api.Interfaces;
using DDStudy2022.Api.Models.Attaches;
using DDStudy2022.Common.Consts;
using DDStudy2022.Common.Exstensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DDStudy2022.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
[ApiExplorerSettings(GroupName = "Api")]
public class AttachController : ControllerBase
{
    private readonly IAttachService _attachService;
    private readonly IPostService _postService;
    private readonly IUserService _userService;

    public AttachController(IAttachService attachService, IPostService postService, IUserService userService)
    {
        _attachService = attachService;
        _postService = postService;
        _userService = userService;
    }

    private FileStreamResult RenderAttach(AttachModel attach, bool download)
    {
        var fs = new FileStream(attach.FilePath, FileMode.Open);
        var ext = Path.GetExtension(attach.Name);

        if (download)
        {
            return File(fs, attach.MimeType, $"{attach.Id}{ext}");
        }

        return File(fs, attach.MimeType);
    }

    [HttpGet]
    [Route("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileStreamResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<FileStreamResult> GetUserAvatar(Guid userId, bool download = false)
        => RenderAttach(await _userService.GetUserAvatar(userId), download);

    [HttpGet]
    [Route("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileStreamResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<FileStreamResult> GetCurrentUserAvatar(Guid userId, bool download = false)
        => await GetUserAvatar(User.GetClaimValue<Guid>(ClaimNames.Id), download);

    [HttpGet]
    [Route("{postContentId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileStreamResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<FileStreamResult> GetPostContent(Guid postContentId, bool download = false)
        => RenderAttach(await _postService.GetPostContent(postContentId), download);

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MetadataModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<List<MetadataModel>> UploadFiles([FromForm] List<IFormFile> files)
    {
        var res = new List<MetadataModel>();
        foreach (var file in files)
        {
            res.Add(await _attachService.UploadFile(file));
        }

        return res;
    }
}