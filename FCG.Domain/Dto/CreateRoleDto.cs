namespace FCG.Domain.Dto;

public class CreateRoleDto
{
    public int UserId { get; set; }

    public IEnumerable<string> Roles { get; set; } = [];
}
