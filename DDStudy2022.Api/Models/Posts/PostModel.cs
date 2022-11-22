using DDStudy2022.Api.Models.Attaches;
using DDStudy2022.Api.Models.Comments;
using DDStudy2022.Api.Models.Likes;
using DDStudy2022.Api.Models.Users;

namespace DDStudy2022.Api.Models.Posts;

public class PostModel
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
    public UserAvatarModel  Author { get; set; } = null!;
    public List<AttachExternalModel>? Contents { get; set; } = new();
    public List<CommentModel>? Comments { get; set; } = new();
    public List<PostLikeRequest>? Likes { get; set; } = new();
    
    public int CommentsCount { get; set; }
    public int LikesCount { get; set; }
}