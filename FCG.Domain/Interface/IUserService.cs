using FCG.Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace FCG.Domain.Interface;

public interface IUserService
{
    Task<IActionResult> GetUserById(int userId);

    Task<IActionResult> CreateUser(UserModel userModal);

    Task<IActionResult> DeleteUserById(int userId);
}
