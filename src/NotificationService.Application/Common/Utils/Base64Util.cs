using System;
using System.Text;

namespace NotificationService.Application.Common.Utils;

internal static class Base64Util
{
    public static string EncodeBase64(this string text)
    {
        var textAsBytes = Encoding.UTF8.GetBytes(text);
        return Convert.ToBase64String(textAsBytes);
    }

    public static string DecodeBase64(this string base64Encoded)
    {
        if (!IsBase64String(base64Encoded))
            return string.Empty;
        
        var textAsBytes = Convert.FromBase64String(base64Encoded);
        return Encoding.UTF8.GetString(textAsBytes);
    }

    private static bool IsBase64String(string base64)
    {
       Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
       return Convert.TryFromBase64String(base64, buffer , out _);
    }
}