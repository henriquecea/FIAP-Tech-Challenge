using FCG.Domain.Configuration;
using FCG.Domain.Entity;
using FCG.Domain.Entity.ValueObject;
using FCG.Domain.Interface.Repository;
using FCG.Domain.Interface.Service;
using FCG.Domain.Model;
using FCG.Domain.ValueObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SecureIdentity.Password;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FCG.Application.Service;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    private readonly JwtSettings _jwtSettings;

    public UserService(IUserRepository userRepository,
                       IConfiguration configuration,
                       IOptions<JwtSettings> jwtSettings)
    {
        _userRepository = userRepository;
        _configuration = configuration;

        _jwtSettings = jwtSettings.Value;
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

            return new OkObjectResult("Usuário registrado com sucesso!");
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

    public async Task<IActionResult> AuthenticateUser(UserModel userRequest)
    {
        try
        {
            var user = await _userRepository.GetByEmailAsync(userRequest.EmailAddress);

            if (user == null)
                return new NotFoundObjectResult("O usúario não existe.");

            var hashPassword = PasswordHasher.Hash(userRequest.Password);

            if (PasswordHasher.Verify(user.Password.Hash, hashPassword))
                return new BadRequestObjectResult("Usuário ou senha incorretos.");

            return new OkObjectResult(GenerateToken(user));
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(ex.Message);
        }
    }

    private string GenerateToken(UserEntity user)
    {
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.EmailAddress.Address)
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
