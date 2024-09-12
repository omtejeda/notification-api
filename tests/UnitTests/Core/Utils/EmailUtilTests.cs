using NotificationService.Application.Common.Utils;
using NotificationService.Common.Dtos;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Enums;
using NotificationService.Common.Exceptions;

namespace NotificationService.Application.Tests.Utils;

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
    public static TheoryData<string, List<MetadataDto>, string> GetReplaceParametersTestData()
    {
        var metadataForFirstTestCase = new List<MetadataDto>
        {
            new () { Key = "username", Value = "John Doe" }
        };

        var metadataForSecondTestCase = new List<MetadataDto>
        {
            new () { Key = "PLATFORM_NAME", Value = "ClientExpenses" },
            new () { Key = "Link", Value = "https://our-dummy-website.com/" }
        };

        return new()
        {
            { 
                "Hi, $[username]",
                metadataForFirstTestCase,
                "Hi, John Doe" 
            },
            { 
                "Welcome to our platform $[PLATFORM_NAME]. Hit the following link to know more about us! $[Link]",
                metadataForSecondTestCase,
                "Welcome to our platform ClientExpenses. Hit the following link to know more about us! https://our-dummy-website.com/"
            }
        };
    }

    public static TheoryData<string, Provider, string, Type> GetThrowIfEmailNotAllowedTestData()
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
                AllowedRecipients =
                [
                    "johndoe@github.com",
                    "johndoe@gmail.com",
                    "johndoe@clientexpenses.com"
                ]
            }
        };

        return new()
        {
            { "Development", provider, "johndoe@github.com", null },
            { "Development", provider, "davidb@user.com", typeof(RuleValidationException) },
            { "Staging", provider, "my-custom-user@my-custom-domain.com", typeof(RuleValidationException) },
            { "Production", provider, "my-custom-user@my-custom-domain.com", null }
        };
    }
    #endregion
}