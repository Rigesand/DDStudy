using DDStudy2022.Api.Interfaces;
using DDStudy2022.Api.Models.Comments;
using DDStudy2022.Api.Models.Likes;
using DDStudy2022.Api.Models.Posts;
using DDStudy2022.Api.Services;
using DDStudy2022.Common.Consts;
using DDStudy2022.Common.Exceptions;
using DDStudy2022.Common.Exstensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DDStudy2022.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
[ApiExplorerSettings(GroupName = "Api")]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly ILikeService _likeService;
    private readonly ICommentService _commentService;

    public PostController(
        IPostService postService,
        LinkGeneratorService links,
        ICommentService commentService,
        ILikeService likeService)
    {
        _postService = postService;
        _commentService = commentService;
        _likeService = likeService;
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PostModel>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IEnumerable<PostModel>> GetPosts(int skip = 0, int take = 10)
        => await _postService.GetPosts(skip, take);

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<PostModel> GetPostById(Guid id)
        => await _postService.GetPostById(id);

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        var userId = User.GetClaimValue<Guid>(ClaimNames.Id);
        if (userId != default)
        {
            await _commentService.CreateComment(createComment, userId);
        }
        else
            throw new UserException("You are not authorized");
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task ChangeLikeFromPost([FromBody] PostLikeRequest likeRequest)
    {
        if (!likeRequest.UserId.HasValue)
        {
            var userId = User.GetClaimValue<Guid>(ClaimNames.Id);
            if (userId == default)
                throw new UserException("not authorize");
            likeRequest.UserId = userId;
        }

        await _likeService.ChangeLikeFromPost(likeRequest);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task ChangeLikeFromComment([FromBody] CommentLikeRequest likeRequest)
    {
        if (!likeRequest.UserId.HasValue)
        {
            var userId = User.GetClaimValue<Guid>(ClaimNames.Id);
            if (userId == default)
                throw new UserException("not authorize");
            likeRequest.UserId = userId;
        }

        await _likeService.ChangeLikeFromComment(likeRequest);
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task DeleteComment(Guid id)
    {
        var userId = User.GetClaimValue<Guid>(ClaimNames.Id);
        if (userId == default)
            throw new UserException("not authorize");

        await _commentService.DeleteComment(id, userId);
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task DeletePost(Guid id)
    {
        var userId = User.GetClaimValue<Guid>(ClaimNames.Id);
        if (userId == default)
            throw new UserException("not authorize");

        await _postService.DeletePost(id, userId);
    }
}