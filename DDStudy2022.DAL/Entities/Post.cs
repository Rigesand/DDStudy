namespace DDStudy2022.DAL.Entities;

public class Post
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
    public Guid AuthorId { get; set; }
    public User Author { get; set; } = null!;
    public DateTimeOffset Created { get; set; }

    public ICollection<PostContent>? PostContents { get; set; }
    public ICollection<Comment>? Comments { get; set; } = new List<Comment>();
}