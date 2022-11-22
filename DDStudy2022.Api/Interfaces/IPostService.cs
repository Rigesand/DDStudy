using DDStudy2022.Api.Models.Attaches;
using DDStudy2022.Api.Models.Posts;

namespace DDStudy2022.Api.Interfaces;

public interface IPostService
{
    public Task CreatePost(CreatePostRequest newPost);
    public Task<AttachModel> GetPostContent(Guid postContentId);
    public Task<IEnumerable<PostModel>> GetPosts(int skip, int take);
    public Task<PostModel> GetPostById(Guid id);

    public Task DeletePost(Guid id, Guid userId);
}