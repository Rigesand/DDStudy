using DDStudy2022.Api.Interfaces;
using DDStudy2022.Api.Models.Tokens;
using DDStudy2022.Api.Models.Users;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace DDStudy2022.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly IValidator<CreateUserModel> _createValidator;

    public AuthController(ITokenService tokenService, IValidator<CreateUserModel> createValidator)
    {
        _tokenService = tokenService;
        _createValidator = createValidator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<TokenModel> Login([FromBody] TokenRequestModel model)
    {
        return await _tokenService.Login(model.Login!, model.Password!);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task Registration([FromBody] CreateUserModel createUserModel)
    {
        await _createValidator.ValidateAndThrowAsync(createUserModel);
        await _tokenService.Registration(createUserModel);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<TokenModel> RefreshToken([FromBody] RefreshTokenRequestModel model)
    {
        return await _tokenService.GetTokenByRefreshToken(model.RefreshToken!);
    }
}