namespace DDStudy2022.Api.Models.Likes;

public class CommentLikeRequest: LikeRequest
{
    public Guid CommentId { get; set; }
}