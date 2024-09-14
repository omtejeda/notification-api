using NotificationService.Application.Utils;

namespace NotificationService.Application.Tests.Utils;

public class SecretMaskerTests
{
    [Theory]
    [InlineData("myCustomApiKey", "myCu******iKey")]
    [InlineData("", "")]
    [InlineData("pw", "**")]
    [InlineData("pass", "****")]
    [InlineData("key", "k*y")]
    public void Mask_ShouldReturnTextMaskedCorrectly(string secret, string expected)
    {
        var actual = SecretMasker.Mask(secret);

        Assert.Equal(expected, actual);
    }
}