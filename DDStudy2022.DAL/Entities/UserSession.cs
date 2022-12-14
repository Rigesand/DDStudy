namespace DDStudy2022.DAL.Entities;

public class UserSession
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid RefreshToken { get; set; }
    public DateTimeOffset Created { get; set; }
    public bool IsActive { get; set; } = true;
}