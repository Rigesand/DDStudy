namespace DDStudy2022.DAL.Entities;

public class CommentLike : Like
{
    public Guid CommentId { get; set; }
    public Comment Comment { get; set; } = null!;
}