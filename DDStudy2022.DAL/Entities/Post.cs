namespace DDStudy2022.DAL.Entities;

public class Post
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public Guid UserAccountId { get; set; }
    public UserAccount UserAccount { get; set; } = null!;
    public ICollection<Photo> Photos { get; set; } = null!;
    public ICollection<Comment> Comments { get; set; } = null!;
}