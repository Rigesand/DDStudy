namespace DDStudy2022.Api.Models.Likes;

public class PostLikeRequest : LikeRequest
{
    public Guid PostId { get; set; }
}