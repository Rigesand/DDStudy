namespace DDStudy2022.Api.Models.Users;

public class UserModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTimeOffset BirthDate { get; set; }
    public Guid AccountId { get; set; }
}