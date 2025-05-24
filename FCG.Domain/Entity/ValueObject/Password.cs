using SecureIdentity.Password;

namespace FCG.Domain.Entity.ValueObject;

public partial class Password
{
    protected Password() { }

    public Password(string? text = null)
    {
        if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
            throw new Exception("Password is invalid or null.");

        Hash = PasswordHasher.Hash(text);
    }

    public string Hash { get; } = string.Empty;
}
