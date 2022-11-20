using System.ComponentModel.DataAnnotations;

namespace DDStudy2022.Api.Models.Users;

public class CreateUserModel
{
    [Required] 
    public string Name { get; set; } = string.Empty;
    [Required] 
    public string Email { get; set; } = string.Empty;
    [Required] 
    public string Password { get; set; } = string.Empty;
    [Required] 
    [Compare(nameof(Password))] 
    public string RetryPassword { get; set; } = string.Empty;
    [Required] 
    public DateTimeOffset BirthDate { get; set; }
}