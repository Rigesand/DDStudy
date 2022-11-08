namespace DDStudy2022.Api.Models.Comments;

public class CommentModel
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public string Content { get; set; } = string.Empty;
    public Guid UserAccountId { get; set; }
}