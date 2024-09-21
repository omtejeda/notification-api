using NotificationService.Domain.Exceptions;
using NotificationService.SharedKernel;

namespace NotificationService.Domain.ValueObjects;

public class Email : ValueObject
{
    private const char At = '@';
    private const char Dot = '.';
    private const char Space = ' ';
    private const int MaxEmailLength = 254;

    public string Value { get; }
    public string LocalPart => Value[..Value.IndexOf(At)];
    public string Domain => Value[(Value.IndexOf(At) + 1)..];

    public static Email From(string value) => new(value);
    public static implicit operator string(Email email) => email.Value;
    public static implicit operator Email(string value) => From(value);

    private Email(string value)
    {
        var emailValue = value
            .Trim()
            .ToLower();

        Validate(emailValue);
        Value = emailValue;
    }

    private static void Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new EmailInvalidException("Email cannot be empty.");

        if (value.Length > MaxEmailLength)
            throw new EmailInvalidException($"Email cannot exceed {MaxEmailLength} characters.");

        if (value.Contains(Space))
            throw new EmailInvalidException("Email cannot contain spaces.");

        if (!value.Contains(At))
            throw new EmailInvalidException($"Email must contain an {At} symbol.");
            
        if (!value.Contains(Dot))
            throw new EmailInvalidException($"Email must contain a {Dot} symbol.");
        
        if (value.IndexOf(At) > value.LastIndexOf(Dot))
            throw new EmailInvalidException($"Email must contain {Dot} placed after {At}");
        
        if (value.Count(x => x == At) != 1)
            throw new EmailInvalidException("Email must contain exactly one '@' symbol.");
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}