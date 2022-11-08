namespace DDStudy2022.DAL.Entities;

public class Comment
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public string Content { get; set; } = string.Empty;
    public Guid UserAccountId { get; set; }
    public UserAccount UserAccount { get; set; } = null!;
    public Guid PostId { get; set; }
    public Post Post { get; set; } = null!;
}