using FCG.Domain.Interface.Service;
using FCG.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FCG.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize]
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUser([FromRoute] int userId) =>
        await _userService.GetUserById(userId);

    [Authorize]
    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser([FromRoute] int userId) =>
        await _userService.DeleteUserById(userId);
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(UserModel userRequest) =>
        await _userService.CreateUser(userRequest);

    [HttpPost("authentication")]
    public async Task<IActionResult> AuthenticationUser(UserModel userRequest) =>
        await _userService.AuthenticateUser(userRequest);
}
