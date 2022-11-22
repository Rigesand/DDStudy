using DDStudy2022.Api.Models.Likes;

namespace DDStudy2022.Api.Interfaces;

public interface ILikeService
{
    public Task ChangeLikeFromComment(CommentLikeRequest newLike);
    public Task ChangeLikeFromPost(PostLikeRequest newLike);
}