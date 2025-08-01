using System.Text.RegularExpressions;

namespace FCG.Domain.ValueObject;

public partial class EmailAddress
{
    private const string Pattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";

    protected EmailAddress() { }

    public EmailAddress(string address)
    {
        if (string.IsNullOrEmpty(address))
            throw new ArgumentNullException(nameof(address), "Email address is required.");

        if (address.Length < 5)
            throw new ArgumentException("Email address is too short.", nameof(address));

        if (!EmailRegex().IsMatch(address))
            throw new ArgumentException("Email is invalid.", nameof(address));

        Address = address.Trim().ToLower();
    }

    public string Address { get; } = null!;

    [GeneratedRegex(Pattern)]
    private static partial Regex EmailRegex();
}
