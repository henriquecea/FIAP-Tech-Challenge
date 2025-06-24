using FCG.Domain.Configuration;
using FCG.Domain.Dto;
using FCG.Domain.Entity;
using FCG.Domain.Entity.ValueObject;
using FCG.Domain.Interface.Repository;
using FCG.Domain.Interface.Service;
using FCG.Domain.Model;
using FCG.Domain.ValueObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    private readonly IRepository<RoleEntity> _roleRepository;

    private readonly ILogger<UserService> _logger;

    private readonly JwtSettings _jwtSettings;

    public UserService(IUserRepository userRepository,
                       IRepository<RoleEntity> roleRepository,
                       IOptions<JwtSettings> jwtSettings,
                       ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _logger = logger;

        _jwtSettings = jwtSettings.Value;
    }

    #region User CRUD

    public async Task<IActionResult> GetUserById(int userId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                return new NotFoundObjectResult("O usúario não existe.");

            _logger.LogInformation("Usuário {Name} encontrado com sucesso!", user.Name);
            return new OkObjectResult(user);
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro ao buscar usuário: {Message}", ex.Message);
            return new BadRequestObjectResult(ex.Message);
        }
    }

    public async Task<IActionResult> CreateUser(UserModel userModal)
    {
        try
        {
            var newUser = new UserEntity(userModal.Name!,
                          new EmailAddress(userModal.EmailAddress),
                          new Password(userModal.Password));

            await _userRepository.AddAsync(newUser);
            await _userRepository.SaveChangesAsync();

            _logger.LogInformation("Usuário {Name} criado com sucesso!", newUser.Name);
            return new OkObjectResult("Usuário registrado com sucesso!");
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro ao criar usuário: {Message}", ex.Message);
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

            _logger.LogInformation("Usuário {Name} deletado com sucesso!", user.Name);
            return new OkObjectResult("Usuário deletado com sucesso!");
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro ao deletar usuário: {Message}", ex.Message);
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

            _logger.LogInformation("Usuário {Name} autenticado com sucesso!", user.Name);
            return new OkObjectResult(GenerateToken(user));
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro ao autenticar usuário: {Message}", ex.Message);
            return new BadRequestObjectResult(ex.Message);
        }
    }

    private string GenerateToken(UserEntity user)
    {
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var roleClaims = user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name));

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.EmailAddress.Address)
        }
         .Union(roleClaims)
         .ToList();

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    #endregion

    #region Roles CRUD

    public async Task<IActionResult> AttributeRoles(CreateRoleDto roles)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(roles.UserId);

            if (user is null)
                return new NotFoundObjectResult("O usuário não existe.");

            //await _roleRepository.AddAsync(new RoleEntity()
            //{
            //    Users = user.Id
            //});

            _logger.LogInformation("Roles adicionadas para o usuário {Name}", user.Name);
            return new OkObjectResult($"Roles adicionadas ao usuário {user.Name}!");
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro ao adicionar Roles no usuário: {Message}", ex.Message);
            return new BadRequestObjectResult(ex.Message);
        }
    }

    public async Task<IActionResult> GetAllRoles()
    {
        try
        {
            var roles = await _roleRepository.GetAllAsync();

            return new OkObjectResult(roles.Select(role => new
            {
                role.Id,
                role.Name,
            }));
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro ao buscar Roles: {Message}", ex.Message);
            return new BadRequestObjectResult(ex.Message);
        }
    }

    public async Task<IActionResult> CreateRoles(CreateRoleDto roles)
    {
        try
        {
            return new OkObjectResult("Método CreateRoles não implementado ainda.");
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro ao criar nova Role: {Message}", ex.Message);
            return new BadRequestObjectResult(ex.Message);
        }
    }

    #endregion
}
