using System.ComponentModel.DataAnnotations;

namespace DDStudy2022.Api.Models.Users;

public class UpdateUser
{
    [Required]
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    [Compare(nameof(Password))]
    public string? RetryPassword { get; set; }
    public DateTimeOffset BirthDate { get; set; }
}