using NotificationService.Domain.Exceptions;
using NotificationService.SharedKernel.Domain;

namespace NotificationService.Domain.ValueObjects;

public class PhoneNumber : ValueObject
{
    private const int MinLength = 10;
    private const int MaxLength = 15;
    private const string Space = " ";
    private const string Hyphen = "-";
    private const string OpenParenthesis = "(";
    private const string CloseParenthesis = ")";
    private const char PlusSign = '+';

    public string Value { get; }

    private PhoneNumber(string value)
    {
        var phoneNumberValue = Normalize(value);
        
        Validate(phoneNumberValue);
        Value = phoneNumberValue;
    }

    public static PhoneNumber From(string value) => new(value);
    public static implicit operator string(PhoneNumber phoneNumber) => phoneNumber.Value;
    public static implicit operator PhoneNumber(string phoneNumber) => From(phoneNumber);

    private static string Normalize(string value)
    {
        return value
            .Trim()
            .TrimStart(PlusSign)
            .Replace(Space, string.Empty)
            .Replace(Hyphen, string.Empty)
            .Replace(OpenParenthesis, string.Empty)
            .Replace(CloseParenthesis, string.Empty);
    }

    private static void Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new PhoneNumberInvalidException("Phone number cannot be empty.");

        if (value.Length < MinLength || value.Length > MaxLength)
            throw new PhoneNumberInvalidException($"Phone number must be between {MinLength} and {MaxLength} digits.");

        if (!value.All(char.IsDigit))
            throw new PhoneNumberInvalidException("Phone number can only contain digits.");
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}