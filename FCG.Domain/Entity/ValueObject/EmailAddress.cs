﻿using System.Text.RegularExpressions;

namespace FCG.Domain.ValueObject;

public partial class EmailAddress
{
    private const string Pattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";

    protected EmailAddress() { }

    public EmailAddress(string address)
    {
        if (string.IsNullOrEmpty(address))
            throw new Exception("Email address is required");

        Address = address.Trim().ToLower();

        if (Address.Length < 5)
            throw new Exception("Email address is too short");

        if (!EmailRegex().IsMatch(Address))
            throw new Exception("Email is invalid");
    }

    public string Address { get; } = null!;

    [GeneratedRegex(Pattern)]
    private static partial Regex EmailRegex();
}
