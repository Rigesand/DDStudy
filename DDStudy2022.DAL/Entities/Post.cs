namespace DDStudy2022.DAL.Entities;

public class Post
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public string Content { get; set; } = string.Empty;
    public Guid UserAccountId { get; set; }
    public UserAccount UserAccount { get; set; } = null!;
    public ICollection<Photo>? Photos { get; set; } = new List<Photo>();
    public ICollection<Comment>? Comments { get; set; } = new List<Comment>();
}