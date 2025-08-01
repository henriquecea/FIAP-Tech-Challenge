using Newtonsoft.Json;

namespace FCG.Domain.Dto;

public class CreateRoleDto
{
    [JsonProperty("user_id")]
    public required Guid UserId { get; set; }

    [JsonProperty("roles_id")]
    public IEnumerable<Guid> Roles { get; set; } = [];
}
