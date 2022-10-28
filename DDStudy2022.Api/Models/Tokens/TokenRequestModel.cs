using System.ComponentModel.DataAnnotations;

namespace DDStudy2022.Api.Models.Tokens;

public class TokenRequestModel
{
    [Required]
    public string? Login { get; set; }
    [Required]
    public string? Password { get; set; }
}