namespace FCG.Domain.Dto;

public sealed class UserDto
{
    public Guid UserId { get; set; } = Guid.Empty;

    public string Name { get; set; } = default!;

    public string EmailAddress { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public IEnumerable<RoleDto> Roles { get; set; } = [];
}
