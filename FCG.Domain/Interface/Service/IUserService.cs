using FCG.Domain.Dto;
using FCG.Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace FCG.Domain.Interface.Service;

public interface IUserService
{
    #region User CRUD 

    /// <summary>
    /// Retrieves a user by their ID.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve.</param>
    /// <returns>The action result containing the requested user.</returns>
    Task<IActionResult> GetUserById(Guid userId);

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="userModal">The user data to create.</param>
    /// <returns>The action result of the user creation.</returns>
    Task<IActionResult> CreateUser(UserModel userModal);

    /// <summary>
    /// Deletes a user by their ID.
    /// </summary>
    /// <param name="userId">The ID of the user to delete.</param>
    /// <returns>The action result of the deletion.</returns>
    Task<IActionResult> DeleteUserById(Guid userId);

    /// <summary>
    /// Authenticates a user with the provided credentials.
    /// </summary>
    /// <param name="userRequest">The user data for authentication.</param>
    /// <returns>The action result of the authentication process.</returns>
    Task<IActionResult> AuthenticateUser(UserModel userRequest);

    /// <summary>
    /// Get All Users
    /// </summary>
    /// <returns></returns>
    Task<IActionResult> GetAllUsers();
    
    #endregion

    #region Roles CRUD

    /// <summary>
    /// Assigns roles to a user.
    /// </summary>
    /// <param name="roles">The roles data to assign.</param>
    /// <returns>The action result of the role assignment.</returns>
    Task<IActionResult> AttributeRoles(CreateRoleDto roles);

    /// <summary>
    /// Retrieves all available roles.
    /// </summary>
    /// <returns>The action result containing the list of roles.</returns>
    Task<IActionResult> GetAllRoles();

    /// <summary>
    /// Creates new roles.
    /// </summary>
    /// <param name="roles">The roles data to create.</param>
    /// <returns>The action result of the role creation.</returns>
    Task<IActionResult> CreateRoles(IEnumerable<string> rolesName);

    /// <summary>
    /// Delete roles by their IDs.
    /// </summary>
    /// <param name="rolesID"></param>
    /// <returns></returns>
    Task<IActionResult> DeleteRoles(IEnumerable<Guid> rolesID);

    /// <summary>
    /// Deletes roles from a user.
    /// </summary>
    /// <param name="roles"></param>
    /// <returns></returns>
    Task<IActionResult> DeleteAttributeRole(CreateRoleDto data);

    #endregion
}

