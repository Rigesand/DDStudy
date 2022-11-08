using DDStudy2022.Api.Models.Attaches;
using DDStudy2022.Api.Models.Comments;
using DDStudy2022.Api.Models.Posts;

namespace DDStudy2022.Api.Interfaces;

public interface IUserAccountService
{
    public Task AddAvatarToUser(Guid userAccountId, MetadataModel meta, string filePath);
    public Task CreatePost(CreatePostModel createPostModel, Guid userAccountId);
    public Task<AttachModel> GetUserAvatar(Guid userAccountId);
    public Task<GetPostModel> GetPost(Guid postId);
    public Task CreateComment(CreateCommentModel newComment, Guid userAccountId);
    public Task<IEnumerable<CommentModel>> GetAllComments(Guid postId);
}