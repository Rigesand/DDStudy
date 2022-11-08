namespace DDStudy2022.DAL.Entities;

public class UserAccount
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Avatar? Avatar { get; set; }
    public ICollection<Post> Posts { get; set; } = null!;
    public ICollection<Comment> Comments { get; set; } = null!;
}