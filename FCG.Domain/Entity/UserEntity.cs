using FCG.Domain.Entity.ValueObject;
using FCG.Domain.ValueObject;

namespace FCG.Domain.Entity;

public class UserEntity : BaseEntity
{
    protected UserEntity() { }

    public UserEntity(string name, EmailAddress emailAddress, Password password)
    {
        Name = name;
        EmailAddress = emailAddress;
        Password = password;
    }

    public string Name { get; set; } = null!;

    public EmailAddress EmailAddress { get; set; } = null!;

    public Password Password { get; set; } = null!;

    public ICollection<RoleEntity> Roles { get; set; } = new List<RoleEntity>();
}
