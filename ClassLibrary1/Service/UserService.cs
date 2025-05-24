using FCG.Domain.Entity;
using FCG.Domain.Entity.ValueObject;
using FCG.Domain.Interface;
using FCG.Domain.Model;
using FCG.Domain.ValueObject;
using Microsoft.AspNetCore.Mvc;

namespace FCG.Application.Service;

public class UserService : IUserService
{
    private readonly IRepository<UserEntity> _userRepository;

    public UserService(IRepository<UserEntity> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IActionResult> GetUserById(int userId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                return new NotFoundObjectResult("O usúario não existe.");

            return new OkObjectResult(user);
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(ex.Message);
        }
    }
    public async Task<IActionResult> CreateUser(UserModel userModal)
    {
        try
        {
            var newUser = new UserEntity(userModal.Name,
                     new EmailAddress(userModal.EmailAddress),
                     new Password(userModal.Password));

            await _userRepository.AddAsync(newUser);
            await _userRepository.SaveChangesAsync();

            return new OkObjectResult(newUser);
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(ex.Message);
        }
    }

    public async Task<IActionResult> DeleteUserById(int userId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                return new NotFoundObjectResult("O usúario não existe.");

            _userRepository.Delete(user);
            await _userRepository.SaveChangesAsync();

            return new OkObjectResult("Usuário deletado com sucesso!");
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(ex.Message);
        }
    }
}
