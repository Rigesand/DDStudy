using DDStudy2022.Api.Interfaces;
using DDStudy2022.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace DDStudy2022.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task CreateUser([FromBody] CreateUserModel createUserModel)
    {
        var isExist = await _userService.FindByMail(createUserModel);
        if (isExist)
        {
            return;
        }

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