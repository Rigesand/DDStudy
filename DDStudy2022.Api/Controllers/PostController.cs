using DDStudy2022.Api.Interfaces;
using DDStudy2022.Api.Models.Comments;
using DDStudy2022.Api.Models.Posts;
using DDStudy2022.Api.Services;
using DDStudy2022.Common.Consts;
using DDStudy2022.Common.Exceptions;
using DDStudy2022.Common.Exstensions;
using Microsoft.AspNetCore.Mvc;

namespace DDStudy2022.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[ApiExplorerSettings(GroupName = "Api")]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;

    public PostController(IPostService postService, LinkGeneratorService links)
    {
        _postService = postService;
        links.LinkContentGenerator = x => Url.ControllerAction<AttachController>(
            nameof(AttachController.GetPostContent), new
            {
                postContentId = x.Id,
            });
        links.LinkAvatarGenerator = x => Url.ControllerAction<AttachController>(nameof(AttachController.GetUserAvatar),
            new
            {
                userId = x.Id,
            });
    }

    [HttpGet]
    public async Task<List<PostModel>> GetPosts(int skip = 0, int take = 10)
        => await _postService.GetPosts(skip, take);

    [HttpGet]
    public async Task<PostModel> GetPostById(Guid id)
        => await _postService.GetPostById(id);

    [HttpPost]
    public async Task CreatePost(CreatePostRequest request)
    {
        if (!request.AuthorId.HasValue)
        {
            var userId = User.GetClaimValue<Guid>(ClaimNames.Id);
            if (userId == default)
                throw new UserException("not authorize");
            request.AuthorId = userId;
        }

        await _postService.CreatePost(request);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task CreateComment([FromBody] CreateCommentModel createComment)
    {
        var userAccountId = User.GetClaimValue<Guid>(ClaimNames.Id);
        if (userAccountId != default)
        {
            await _postService.CreateComment(createComment, userAccountId);
        }
        else
            throw new UserException("You are not authorized");
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CommentModel>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IEnumerable<CommentModel>> GetAllComments(Guid postId)
    {
        return await _postService.GetAllComments(postId);
    }
}