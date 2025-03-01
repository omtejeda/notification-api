using NotificationService.Application.Contracts.Services;
using NotificationService.Application.Features.Providers.Interfaces;
using NotificationService.SharedKernel.Interfaces;
using NotificationService.Domain.Enums;
using NotificationService.Application.Common.Dtos;
using NotificationService.Domain.Models;
using NotificationService.Domain.Entities;
using NotificationService.Application.Exceptions;
using NotificationService.Application.Common;
using NotificationService.Application.Contracts.Senders;
using NotificationService.Application.Contracts.Persistence;
using Moq;
using System.Linq.Expressions;
using NotificationService.Application.Features.Senders.Dtos;
using NotificationService.Application.Features.Senders.Commands.SendSms;

namespace NotificationService.Application.Tests.Senders;

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
        var exception = await Assert.ThrowsAsync<ProviderException>(() => _smsSender.SendSmsAsync(request, request.Template.PlatformName));
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
        var exception = await Assert.ThrowsAsync<ProviderException>(() => _smsSender.SendSmsAsync(request, request.Template.PlatformName));
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
            Template = new Application.Features.Senders.Dtos.TemplateDto
            {
                Name = "MyCustomTemplate",
                PlatformName = "MyCustomPlatform",
                Metadata = new List<Domain.Dtos.MetadataDto>
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
            ProvidedMetadata = new List<Domain.Dtos.MetadataDto>
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
                It.IsAny<List<Domain.Dtos.MetadataDto>>(),
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
        var expectedResult = success 
            ? NotificationResult.Ok((int) ResultCode.OK, "Success")
            : NotificationResult.Fail((int) ResultCode.HttpRequestNotSent, "An error ocurred trying to send HTTP request");
        
        _httpClientProviderMock
            .Setup(m => m.SendHttpClient(
                It.IsAny<HttpClientSetting>(),
                It.IsAny<string>(),
                It.IsAny<ICollection<Domain.Dtos.MetadataDto>>(),
                It.IsAny<string>()))
            .ReturnsAsync(expectedResult);
    }
    #endregion
}