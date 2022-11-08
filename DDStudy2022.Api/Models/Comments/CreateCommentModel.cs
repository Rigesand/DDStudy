namespace DDStudy2022.Api.Models.Comments;

public class CreateCommentModel
{
    public string Content { get; set; } = string.Empty;
    public Guid PostId { get; set; }
}