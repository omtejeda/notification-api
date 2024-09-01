namespace NotificationService.Core.Common.Utils;

public static class SecretMasker
{
    private const int MaxVisibleCharacter = 4;
    private const char MaskCharacter = '*';

    /// <summary>
    /// Masks a secret string by keeping a certain number of characters visible on each end and masking the rest.
    /// </summary>
    /// <param name="secret">The secret string to be masked.</param>
    /// <returns>The masked string.</returns>
    internal static string Mask(string secret)
    {
        int visibleCharacters = Math.Min(secret.Length / 2, MaxVisibleCharacter);
        int totalVisibleCharacters = visibleCharacters * 2;

        if (secret.Length <= totalVisibleCharacters)
        {
            return new string(MaskCharacter, secret.Length);
        }
        
        string firstPart = secret[..visibleCharacters];
        string lastPart = secret[^visibleCharacters..];
        string middleMask = new(MaskCharacter, secret.Length - totalVisibleCharacters);

        return $"{firstPart}{middleMask}{lastPart}";
    }
}