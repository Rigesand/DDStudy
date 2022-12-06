namespace DDStudy2022.DAL.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "empty";
    public string Email { get; set; } = "empty";
    public string About { get; set; } = "empty";
    public string PasswordHash { get; set; } = "empty";
    public DateTimeOffset BirthDay { get; set; }

    public Avatar? Avatar { get; set; }
    public ICollection<UserSession>? Sessions { get; set; }
    public ICollection<Post>? Posts { get; set; }
    public ICollection<Comment>? Comments { get; set; }

    public ICollection<Subscription>? Subscriptions { get; set; }
    public ICollection<Subscription>? Subscribers { get; set; }
}