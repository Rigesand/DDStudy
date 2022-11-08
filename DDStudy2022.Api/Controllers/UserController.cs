using DDStudy2022.Api.Interfaces;
using DDStudy2022.Api.Models.Users;
using DDStudy2022.Common.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DDStudy2022.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserModel>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IEnumerable<UserModel>> GetAllUsers()
    {
        return await _userService.GetAllUsers();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<UserModel> GetCurrentUser()
    {
        var userIdString = User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
        if (Guid.TryParse(userIdString, out var userId))
        {
            return await _userService.GetUser(userId);
        }

        throw new UserException("You are not authorized");
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task DeleteUser(Guid id)
    {
        await _userService.DeleteUser(id);
    }
}