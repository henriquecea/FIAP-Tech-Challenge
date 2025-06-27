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

    public async Task<IActionResult> GetUserById(Guid userId)
    {
        try
        {
            var user = await _userRepository.GetAllUser(userId);

            if (user == null)
                return new NotFoundObjectResult("O usúario não existe.");

            _logger.LogInformation("Usuário {Name} encontrado com sucesso!", user.Name);

            return new OkObjectResult(new UserDto()
            {
                UserId = user.Id,
                Name = user.Name,
                Roles = user.Roles.Select(role => new RoleDto
                {
                    Id = role.Id,
                    Name = role.Name
                }).ToList()
            });
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

    public async Task<IActionResult> DeleteUserById(Guid userId)
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

    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var users = await _userRepository.GetAllAsync();

            return new OkObjectResult(users.Select(user => new
            {
                user.Id,
                user.Name,
                user.EmailAddress.Address,
                user.CreatedAt
            }));
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro ao buscar Roles: {Message}", ex.Message);
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
        var response = new List<string>();

        try
        {
            var user = await _userRepository.GetByIdAsync(roles.UserId);

            if (user is null)
                return new NotFoundObjectResult("O usuário não existe.");

            foreach (var item in roles.Roles)
            {
                var role = await _roleRepository.GetByIdAsync(item);

                if (role == null)
                {
                    response.Add($"Role com ID: '{item}' não existe.");
                    continue;
                }

                if (!user.Roles.Any(r => r.Id == role.Id))
                {
                    user.Roles.Add(role);
                    response.Add($"Role '{role.Name}' adicionada ao usuário {user.Name} com sucesso!");
                }
            }

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            _logger.LogInformation("Roles adicionadas para o usuário {Name}", user.Name);
            return new OkObjectResult(response);
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

    public async Task<IActionResult> CreateRoles(IEnumerable<string> rolesName)
    {
        var response = new List<string>();

        try
        {
            foreach (var item in rolesName)
            {
                await _roleRepository.AddAsync(new RoleEntity() { Name = item });

                response.Add($"Role '{item}' criada com sucesso!");
            }

            await _roleRepository.SaveChangesAsync();

            return new OkObjectResult(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro ao criar nova Role: {Message}", ex.Message);
            return new BadRequestObjectResult(ex.Message);
        }
    }

    public async Task<IActionResult> DeleteRoles(IEnumerable<Guid> rolesID)
    {
        var response = new List<string>();

        try
        {
            foreach (var item in rolesID)
            {
                var role = await _roleRepository.GetByIdAsync(item);

                if (role == null)
                {
                    response.Add($"Role com ID: '{item}' não existe.");
                    continue;
                }

                _logger.LogInformation("Role '{Name}' deletada com sucesso!", role.Name);

                _roleRepository.Delete(role);

                response.Add($"Role '{role.Name}' deletada com sucesso!");
            }

            await _roleRepository.SaveChangesAsync();

            return new OkObjectResult(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro ao criar nova Role: {Message}", ex.Message);
            return new BadRequestObjectResult(ex.Message);
        }
    }


    #endregion
}
