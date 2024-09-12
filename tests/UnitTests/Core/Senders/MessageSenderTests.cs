using NotificationService.Application.Contracts.Interfaces.Services;
using NotificationService.Application.Providers.Factories.Interfaces;
using NotificationService.Common.Interfaces;
using NotificationService.Application.Senders;
using NotificationService.Domain.Enums;
using NotificationService.Common.Dtos;
using NotificationService.Domain.Models;
using NotificationService.Domain.Entities;
using NotificationService.Application.Dtos;
using NotificationService.Common.Exceptions;
using NotificationService.Application.Providers.Interfaces;
using NotificationService.Application.Common;
using NotificationService.Application.Interfaces;
using NotificationService.Application.Contracts.Interfaces.Repositories;
using Moq;
using System.Linq.Expressions;

namespace NotificationService.Application.Tests.Senders;

public class MessageSenderTests
{
    private readonly Mock<INotificationsService> _notificationsServiceMock;
    private readonly Mock<IRepository<Provider>> _providerRepositoryMock;
    private readonly Mock<IHttpClientProvider> _httpClientProviderMock;
    private readonly Mock<ITemplateService> _templateServiceMock;
    private readonly Mock<IDateTimeService> _dateTimeServiceMock;
    private readonly Mock<IEnvironmentService> _environmentServiceMock;
    private readonly IMessageSender _messageSender;

    public MessageSenderTests()
    {
        _notificationsServiceMock = new Mock<INotificationsService>();
        _providerRepositoryMock = new Mock<IRepository<Provider>>();
        _httpClientProviderMock = new Mock<IHttpClientProvider>();
        _templateServiceMock = new Mock<ITemplateService>();
        _dateTimeServiceMock = new Mock<IDateTimeService>();
        _environmentServiceMock = new Mock<IEnvironmentService>();
        
        _messageSender = new MessageSender(
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
    public async Task SendMessageAsync_WithAnyResult_ShouldRegisterNotification(bool sendResult)
    {
        // Arrange
        SendMessageRequestDto request = CreateRequestDto();
        SetupValidRuntimeTemplate();
        SetupProvider(request.ProviderName, ProviderType.HttpClient, request.ToDestination);
        SetupHttpClientProvider(success: sendResult);

        // Act
        await _messageSender.SendMessageAsync(request, request.Template.PlatformName);

        // Assert
        _notificationsServiceMock.Verify(x => x.RegisterNotification(It.IsAny<Notification>()), Times.Once);
    }

    [Fact]
    public async Task SendMessageAsync_ShouldThrowException_WhenProviderIsNotHttp()
    {
        // Arrange
        SendMessageRequestDto request = CreateRequestDto();
        request.ProviderName = "MySendGridAccount";
        
        SetupProvider(request.ProviderName, ProviderType.SendGrid);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ProviderException>(() => _messageSender.SendMessageAsync(request, request.Template.PlatformName));
        var expectedMessage = "No suitable provider found";
        
        Assert.Contains(expectedMessage, exception.Message);
    }

    [Fact]
    public async Task SendMessageAsync_ShouldThrowException_WhenProviderDoesNotExist()
    {
        // Arrangge
        SendMessageRequestDto request = CreateRequestDto();
        
        Provider nullProvider = null;
        _providerRepositoryMock
            .Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<Provider, bool>>>()))
            .ReturnsAsync(nullProvider);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ProviderException>(() => _messageSender.SendMessageAsync(request, request.Template.PlatformName));
        var expectedMessage = $"Provider {request.ProviderName} does not exist";
        
        Assert.Contains(expectedMessage, exception.Message);
    }

    [Fact]
    public async Task SendMessageAsync_ShouldThrowException_WhenNotificationTypeIsNotValid()
    {
        // Arrange
        SendMessageRequestDto request = CreateRequestDto();
        request.NotificationType = NotificationType.Email;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotificationException>(() => _messageSender.SendMessageAsync(request, request.Template.PlatformName));

        Assert.Contains("Notification type not allowed", exception.Message);
    }

    #region Setup Methods
    private SendMessageRequestDto CreateRequestDto()
    {
        return new SendMessageRequestDto
        {
            ToDestination = "180098762222",
            NotificationType = NotificationType.WhatsApp,
            ProviderName = "MyCustomProvider",
            Template = new Application.Dtos.TemplateDto
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