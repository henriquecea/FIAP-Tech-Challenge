using FCG.Domain.Dto;
using FCG.Domain.Entity.ElasticSearch;
using FCG.Domain.Interface.Service;
using FCG.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FCG.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService) 
    : ControllerBase
{

    #region User CRUD

    [Authorize(Roles = "Admin")]
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUser([FromRoute] Guid userId) =>
        await userService.GetUserById(userId);

    [Authorize(Roles = "Admin")]
    [HttpGet()]
    public async Task<IActionResult> GetAllUsers() =>
        await userService.GetAllUsers();

    [Authorize(Roles = "Admin")]
    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid userId) =>
        await userService.DeleteUserById(userId);

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(UserModel userRequest) =>
        await userService.CreateUser(userRequest);

    [HttpPost("authentication")]
    public async Task<IActionResult> AuthenticationUser(UserModel userRequest) =>
        await userService.AuthenticateUser(userRequest);

    [HttpGet("logs")]
    public async Task<IReadOnlyCollection<UserLogEntity>> Logs([FromQuery] int page, [FromQuery] int size) =>
        await userService.GetUserLogs(page, size);

    #endregion

    #region Roles CRUD

    [Authorize(Roles = "Admin")]
    [HttpGet("roles")]
    public async Task<IActionResult> GetAllRoles() =>
        await userService.GetAllRoles();

    [Authorize(Roles = "Admin")]
    [HttpPost("roles")]
    public async Task<IActionResult> CreateRoles(IEnumerable<string> rolesName) =>
        await userService.CreateRoles(rolesName);

    [Authorize(Roles = "Admin")]
    [HttpDelete("roles")]
    public async Task<IActionResult> DeleteRoles(IEnumerable<Guid> rolesId) =>
        await userService.DeleteRoles(rolesId);

    [Authorize(Roles = "Admin")]
    [HttpPatch("roles/attribute")]
    public async Task<IActionResult> AttributeRoles(CreateRoleDto roles) =>
        await userService.AttributeRoles(roles);

    [Authorize(Roles = "Admin")]
    [HttpDelete("roles/attribute")]
    public async Task<IActionResult> DeleteAttributeRole(CreateRoleDto roles) =>
        await userService.DeleteAttributeRole(roles);

    #endregion
}
