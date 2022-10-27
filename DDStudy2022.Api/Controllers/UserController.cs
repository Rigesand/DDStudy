using DDStudy2022.Api.Interfaces;
using DDStudy2022.Api.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace DDStudy2022.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IValidator<CreateUserModel> _createValidator;

    public UserController(
        IUserService userService,
        IValidator<CreateUserModel> createValidator)
    {
        _userService = userService;
        _createValidator = createValidator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task CreateUser([FromBody] CreateUserModel createUserModel)
    {
        await _createValidator.ValidateAndThrowAsync(createUserModel);
        await _userService.CreateUser(createUserModel);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserModel>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IEnumerable<UserModel>> GetAllUsers()
    {
        return await _userService.GetAllUsers();
    }
}