namespace DDStudy2022.DAL.Entities;

public class Like
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}