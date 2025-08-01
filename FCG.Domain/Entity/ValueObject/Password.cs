using SecureIdentity.Password;

namespace FCG.Domain.Entity.ValueObject;

public partial class Password
{
    protected Password() { }

    public Password(string text)
    {
        if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Password is invalid or null.", nameof(text));

        Hash = PasswordHasher.Hash(text);
    }

    public string Hash { get; } = string.Empty;
}
