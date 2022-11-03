namespace DDStudy2022.DAL.Entities;

public class User
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }
    public DateTimeOffset BirthDate { get; set; }
    public ICollection<UserSession>? Sessions { get; set; }
}