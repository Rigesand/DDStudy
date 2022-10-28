using System.ComponentModel.DataAnnotations;

namespace DDStudy2022.Api.Models.Tokens;

public class RefreshTokenRequestModel
{
    [Required] 
    public string? RefreshToken { get; set; }
}