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
        Roles = [];
    }

    public string Name { get; set; }

    public EmailAddress EmailAddress { get; set; }

    public Password Password { get; set; }

    public ICollection<RoleEntity>? Roles { get; set; } = new List<RoleEntity>();
}
