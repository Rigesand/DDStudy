using DDStudy2022.Api.Models.Comments;

namespace DDStudy2022.Api.Interfaces;

public interface ICommentService
{
    public Task CreateComment(CreateCommentModel newComment, Guid userId);
    public Task DeleteComment(Guid id, Guid userId);
}