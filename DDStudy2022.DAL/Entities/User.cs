namespace DDStudy2022.DAL.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTimeOffset BirthDate { get; set; }
    public Guid UserAccountId { get; set; }
    public UserAccount UserAccount { get; set; } = null!;
    public ICollection<UserSession> Sessions { get; set; } = null!;
}