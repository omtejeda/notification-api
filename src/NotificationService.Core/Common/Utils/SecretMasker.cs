using System;
namespace NotificationService.Core.Common.Utils;

public static class SecretMasker
{
    internal static string Mask(string secret)
    {
        int maxVisibleCharacter = 4;
        int visibleCharacters = Math.Min(secret.Length / 2, maxVisibleCharacter);
        int totalVisibleCharacters = visibleCharacters * 2;

        if (secret.Length <= totalVisibleCharacters)
            return secret;
        
        char maskCharacter = '*';
        
        string firstPart = secret[..visibleCharacters];
        string lastPart = secret[^visibleCharacters..];
        string middleMask = new(maskCharacter, secret.Length - totalVisibleCharacters);

        return $"{firstPart}{middleMask}{lastPart}";
    }
}