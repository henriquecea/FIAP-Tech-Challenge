namespace FCG.Domain.Entity.ValueObject;

public class RoleEntity : BaseEntity
{
    public RoleEntity(string name)
    {
        Name = name;
    }

    public RoleEntity(string name, ICollection<UserEntity> users)
    {
        Name = name;
        Users = users;
    }

    public string? Name { get; set; }

    public ICollection<UserEntity> Users { get; set; } = new List<UserEntity>();
}
