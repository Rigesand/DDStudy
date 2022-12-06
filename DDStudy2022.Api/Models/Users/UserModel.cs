namespace DDStudy2022.Api.Models.Users;

public class UserModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string About { get; set; } = "empty";
    public DateTimeOffset BirthDate { get; set; }
    public int PostsCount { get; set; }
}