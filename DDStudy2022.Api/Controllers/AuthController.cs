using DDStudy2022.Api.Interfaces;
using DDStudy2022.Api.Models.Tokens;
using Microsoft.AspNetCore.Mvc;

namespace DDStudy2022.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public AuthController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<TokenModel> Login([FromBody] TokenRequestModel model)
    {
        return await _tokenService.Login(model.Login!, model.Password!);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<TokenModel> RefreshToken([FromBody] RefreshTokenRequestModel model)
    {
        return await _tokenService.GetTokenByRefreshToken(model.RefreshToken!);
    }
}