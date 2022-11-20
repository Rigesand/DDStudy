using DDStudy2022.Api.Interfaces;
using DDStudy2022.Api.Models.Attaches;
using DDStudy2022.Api.Models.Comments;
using DDStudy2022.Api.Models.Posts;
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
    private readonly IAttachService _attachService;

    public UserAccountController(IUserAccountService userAccountService, IAttachService attachService)
    {
        _userAccountService = userAccountService;
        _attachService = attachService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task AddAvatarToUser([FromBody] MetadataModel model)
    {
        var userIdString = User.Claims.FirstOrDefault(x => x.Type == "userAccountId")?.Value;
        if (Guid.TryParse(userIdString, out var userAccountId))
        {
            var path = _attachService.GetPath(model);
            await _userAccountService.AddAvatarToUser(userAccountId, model, path);
        }
        else
            throw new UserException("You are not authorized");
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task CreatePost([FromBody] CreatePostModel createPost)
    {
        var userIdString = User.Claims.FirstOrDefault(x => x.Type == "userAccountId")?.Value;
        if (Guid.TryParse(userIdString, out var userAccountId))
        {
            await _userAccountService.CreatePost(createPost, userAccountId);
        }
        else
            throw new UserException("You are not authorized");
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task CreateComment([FromBody] CreateCommentModel createComment)
    {
        var userIdString = User.Claims.FirstOrDefault(x => x.Type == "userAccountId")?.Value;
        if (Guid.TryParse(userIdString, out var userAccountId))
        {
            await _userAccountService.CreateComment(createComment, userAccountId);
        }
        else
            throw new UserException("You are not authorized");
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<FileResult> GetUserAvatar(Guid userAccountId)
    {
        var attach = await _userAccountService.GetUserAvatar(userAccountId);

        return File(System.IO.File.ReadAllBytes(attach.FilePath), attach.MimeType);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetPostModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<GetPostModel> GetPost(Guid postId)
    {
        var post = await _userAccountService.GetPost(postId);

        return post;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [AllowAnonymous]
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

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CommentModel>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IEnumerable<CommentModel>> GetAllComments(Guid postId)
    {
        return await _userAccountService.GetAllComments(postId);
    }
}