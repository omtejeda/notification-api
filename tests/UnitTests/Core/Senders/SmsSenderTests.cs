using NotificationService.Contracts.Interfaces.Services;
using NotificationService.Core.Providers.Factories.Interfaces;
using NotificationService.Common.Interfaces;
using NotificationService.Core.Senders;
using NotificationService.Common.Enums;
using NotificationService.Common.Dtos;
using NotificationService.Common.Models;
using NotificationService.Common.Entities;
using NotificationService.Core.Dtos;
using NotificationService.Common.Exceptions;
using NotificationService.Core.Providers.Interfaces;
using NotificationService.Core.Common;
using NotificationService.Core.Interfaces;
using NotificationService.Contracts.Interfaces.Repositories;
using Moq;
using System.Linq.Expressions;

namespace NotificationService.Core.Tests.Senders;

public class SmsSenderTests
{
    private readonly Mock<INotificationsService> _notificationsServiceMock;
    private readonly Mock<IRepository<Provider>> _providerRepositoryMock;
    private readonly Mock<IHttpClientProvider> _httpClientProviderMock;
    private readonly Mock<ITemplateService> _templateServiceMock;
    private readonly Mock<IDateTimeService> _dateTimeServiceMock;
    private readonly Mock<IEnvironmentService> _environmentServiceMock;
    private readonly ISmsSender _smsSender;

    public SmsSenderTests()
    {
        _notificationsServiceMock = new Mock<INotificationsService>();
        _providerRepositoryMock = new Mock<IRepository<Provider>>();
        _httpClientProviderMock = new Mock<IHttpClientProvider>();
        _templateServiceMock = new Mock<ITemplateService>();
        _dateTimeServiceMock = new Mock<IDateTimeService>();
        _environmentServiceMock = new Mock<IEnvironmentService>();
        
        _smsSender = new SmsSender(
            _notificationsServiceMock.Object,
            _providerRepositoryMock.Object,
            _httpClientProviderMock.Object,
            _templateServiceMock.Object,
            _dateTimeServiceMock.Object,
            _environmentServiceMock.Object
        );
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task SendSmsAsync_WithAnyResult_ShouldRegisterNotification(bool sendResult)
    {
        // Arrange
        SendSmsRequestDto request = CreateRequestDto();
        SetupValidRuntimeTemplate();
        SetupProvider(request.ProviderName, ProviderType.HttpClient, request.ToPhoneNumber);
        SetupHttpClientProvider(success: sendResult);

        // Act
        await _smsSender.SendSmsAsync(request, request.Template.PlatformName);

        // Assert
        _notificationsServiceMock.Verify(x => x.RegisterNotification(It.IsAny<Notification>()), Times.Once);
    }

    [Fact]
    public async Task SendSmsAsync_ShouldThrowException_WhenProviderIsNotHttp()
    {
        // Arrange
        SendSmsRequestDto request = CreateRequestDto();
        request.ProviderName = "MySendGridAccount";
        
        SetupProvider(request.ProviderName, ProviderType.SendGrid);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<RuleValidationException>(() => _smsSender.SendSmsAsync(request, request.Template.PlatformName));
        var expectedMessage = "No suitable provider found";
        
        Assert.Contains(expectedMessage, exception.Message);
    }

    [Fact]
    public async Task SendSmsAsync_ShouldThrowException_WhenProviderDoesNotExist()
    {
        // Arrangge
        SendSmsRequestDto request = CreateRequestDto();
        
        Provider nullProvider = null;
        _providerRepositoryMock
            .Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<Provider, bool>>>()))
            .ReturnsAsync(nullProvider);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<RuleValidationException>(() => _smsSender.SendSmsAsync(request, request.Template.PlatformName));
        var expectedMessage = $"Provider {request.ProviderName} does not exist";
        
        Assert.Contains(expectedMessage, exception.Message);
    }

    #region Setup Methods
    private SendSmsRequestDto CreateRequestDto()
    {
        return new SendSmsRequestDto
        {
            ToPhoneNumber = "180098762222",
            ProviderName = "MyCustomProvider",
            Template = new Core.Dtos.TemplateDto
            {
                Name = "MyCustomTemplate",
                PlatformName = "MyCustomPlatform",
                Metadata = new List<MetadataDto>
                {
                    new() { Key = "ClientName", Value = "John Doe" }
                },
                Language = Language.En
            }
        };
    }

    private void SetupValidRuntimeTemplate()
    {
        RuntimeTemplate runtimeTemplate= new()
        {
            Name = "MyCustomTemplate",
            PlatformName="MyCustomPlatform",
            Language = Language.En,
            ProvidedMetadata = new List<MetadataDto>
            {
                new () { Key = "clientName", Value = "John Doe" }
            },
            Content = "Hi, John Doe",
            Subject = "Greetings"
        };

        _templateServiceMock.Setup(x => 
            x.GetRuntimeTemplate(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<Language>(),
                It.IsAny<List<MetadataDto>>(),
                It.IsAny<string>(),
                It.IsAny<NotificationType>()
            )
        ).ReturnsAsync(runtimeTemplate);
    }

    private void SetupProvider(string name, ProviderType type, string allowedRecipient = null)
    {
        var provider = new Provider
        {
            ProviderId = Guid.NewGuid().ToString(),
            Name = name,
            Type = type,
            Settings = new ProviderSettings
            {
                HttpClient = new HttpClientSetting
                {
                    Host = "my-testing-host.com",
                    Uri = "/send-message",
                    Verb = "POST"
                }
            },
            DevSettings = new ProviderDevSettings
            {
                AllowedRecipients = [ allowedRecipient ]
            }
        };

        _providerRepositoryMock
            .Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<Provider, bool>>>()))
            .ReturnsAsync(provider);
    }

    private void SetupHttpClientProvider(bool success)
    {
        var expectedResult = Tuple.Create(
            success,
            success ? (int) ResultCode.OK : (int) ResultCode.HttpRequestNotSent,
            success ? "Success" : "An error ocurred trying to send HTTP request");
        
        _httpClientProviderMock
            .Setup(m => m.SendHttpClient(
                It.IsAny<HttpClientSetting>(),
                It.IsAny<string>(),
                It.IsAny<ICollection<MetadataDto>>(),
                It.IsAny<string>()))
            .ReturnsAsync(expectedResult);
    }
    #endregion
}