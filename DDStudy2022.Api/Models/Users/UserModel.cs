namespace DDStudy2022.Api.Models.Users;

public class UserModel
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public DateTimeOffset BirthDate { get; set; }
}