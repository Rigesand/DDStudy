namespace DDStudy2022.DAL.Entities;

public class Subscription
{
    public Guid Id { get; set; }
    public Guid SubUserId { get; set; }
    public User SubUser { get; set; } = null!;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}