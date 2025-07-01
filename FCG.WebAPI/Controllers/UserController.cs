using FCG.Domain.Dto;
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
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }

    #region User CRUD

    [Authorize(Roles = "Admin")]
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUser([FromRoute] Guid userId) =>
        await _userService.GetUserById(userId);

    [Authorize(Roles = "Admin")]
    [HttpGet()]
    public async Task<IActionResult> GetAllUsers() =>
        await _userService.GetAllUsers();

    [Authorize(Roles = "Admin")]
    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid userId) =>
        await _userService.DeleteUserById(userId);

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(UserModel userRequest) =>
        await _userService.CreateUser(userRequest);

    [HttpPost("authentication")]
    public async Task<IActionResult> AuthenticationUser(UserModel userRequest) =>
        await _userService.AuthenticateUser(userRequest);

    #endregion

    #region Roles CRUD

    [Authorize(Roles = "Admin")]
    [HttpGet("roles")]
    public async Task<IActionResult> GetAllRoles() =>
        await _userService.GetAllRoles();

    [Authorize(Roles = "Admin")]
    [HttpPost("roles")]
    public async Task<IActionResult> CreateRoles(IEnumerable<string> rolesName) =>
        await _userService.CreateRoles(rolesName);

    [Authorize(Roles = "Admin")]
    [HttpDelete("roles")]
    public async Task<IActionResult> DeleteRoles(IEnumerable<Guid> rolesId) =>
        await _userService.DeleteRoles(rolesId);

    [Authorize(Roles = "Admin")]
    [HttpPatch("roles/attribute")]
    public async Task<IActionResult> AttributeRoles(CreateRoleDto roles) =>
        await _userService.AttributeRoles(roles);

    [Authorize(Roles = "Admin")]
    [HttpDelete("roles/attribute")]
    public async Task<IActionResult> DeleteAttributeRole(CreateRoleDto roles) =>
        await _userService.DeleteAttributeRole(roles);

    #endregion
}
