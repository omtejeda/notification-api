using NotificationService.Core.Common.Utils;
using NotificationService.Common.Dtos;
using NotificationService.Common.Entities;
using NotificationService.Common.Enums;
using NotificationService.Common.Exceptions;

namespace NotificationService.Core.Tests.Utils;

public class EmailUtilTests
{
    [Theory]
    [MemberData(nameof(GetReplaceParametersTestData))]
    public void ReplaceParameters_ShouldReturnTextReplaced(string text, List<MetadataDto> metadata, string expected)
    {
        var actual = EmailUtil.ReplaceParameters(text, metadata);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetThrowIfEmailNotAllowedTestData))]
    public void ThrowIfEmailNotAllowed_ShouldExecuteWithoutThrowing(string environment, Provider provider, string to, Type expectedException)
    {
        if (expectedException is null)
        {
            EmailUtil.ThrowIfEmailNotAllowed(environment, provider, to);
        }
        else
        {
            Assert.Throws(expectedException, () => EmailUtil.ThrowIfEmailNotAllowed(environment, provider, to));
        }
    }
    #region TestData
    public static IEnumerable<object[]> GetReplaceParametersTestData()
    {
        yield return new object[]
        {
            "Hi, $[username]",
            new List<MetadataDto>
            {
                new () { Key = "username", Value = "John Doe" }
            },
            "Hi, John Doe"
        };

        yield return new object[]
        {
            "Welcome to our platform $[PLATFORM_NAME]. Hit the following link to know more about us! $[Link]",
            new List<MetadataDto>
            {
                new () { Key = "PLATFORM_NAME", Value = "ClientExpenses" },
                new () { Key = "Link", Value = "https://our-dummy-website.com/" }
            },
            "Welcome to our platform ClientExpenses. Hit the following link to know more about us! https://our-dummy-website.com/"
        };
    }

    public static IEnumerable<object[]> GetThrowIfEmailNotAllowedTestData()
    {
        var provider = new Provider
        {
            ProviderId = Guid.NewGuid().ToString(),
            Name = "MySendGridAccount",
            Type = ProviderType.SendGrid,
            IsActive = true,
            IsPublic = false,
            DevSettings = new ProviderDevSettings
            {
                AllowedRecipients = new[]
                {"johndoe@github.com", "johndoe@gmail.com", "johndoe@clientexpenses.com"}
            }
        };
        

        yield return new object[]
        {
            "Development",
            provider,
            "johndoe@github.com",
            null
        };

        yield return new object[]
        {
            "Development",
            provider,
            "johndoe@github.com",
            null
        };

        yield return new object[]
        {
            "Development",
            provider,
            "davidb@user.com",
            typeof(RuleValidationException)
        };

        yield return new object[]
        {
            "Staging",
            provider,
            "my-custom-user@my-custom-domain.com",
            typeof(RuleValidationException)
        };

        yield return new object[]
        {
            "Production",
            provider,
            "my-custom-user@my-custom-domain.com",
            null
        };
    }
    #endregion
}