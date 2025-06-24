namespace FCG.Domain.Entity.ValueObject;

public class RoleEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public ICollection<UserEntity> Users { get; set; } = new List<UserEntity>();
}
