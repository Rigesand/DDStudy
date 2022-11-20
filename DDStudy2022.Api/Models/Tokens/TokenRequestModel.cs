using System.ComponentModel.DataAnnotations;

namespace DDStudy2022.Api.Models.Tokens;

public class TokenRequestModel
{
    [Required] 
    public string Login { get; set; } = string.Empty;
    [Required] 
    public string Password { get; set; } = string.Empty;
}