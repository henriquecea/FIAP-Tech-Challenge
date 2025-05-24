using FCG.Domain.Interface;
using FCG.Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace FCG.Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUser([FromRoute] int userId) =>
        await _userService.GetUserById(userId);

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser([FromRoute] int userId) =>
        await _userService.DeleteUserById(userId);
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(UserModel userRequest) =>
        await _userService.CreateUser(userRequest);
}
