using Newtonsoft.Json;

namespace FCG.Domain.Dto;

public class CreateRoleDto
{
    [JsonProperty("user_id")]
    public Guid UserId { get; set; } = default!;

    [JsonProperty("roles_id")]
    public IEnumerable<Guid> Roles { get; set; } = [];
}
