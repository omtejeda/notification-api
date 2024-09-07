
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
using Moq;
using Microsoft.AspNetCore.Http;

namespace NotificationService.Core.Tests.Senders;
public class EmailSenderTests
{
    private readonly Mock<ITemplateService> _templateService;
    private readonly Mock<INotificationsService> _notificationsService;
    private readonly Mock<IEmailProviderFactory> _emailProviderFactory;
    private readonly Mock<IDateTimeService> _dateTimeService;
    private readonly EmailSender _emailSender;

    public EmailSenderTests()
    {
        _templateService = new Mock<ITemplateService>();
        _notificationsService = new Mock<INotificationsService>();
        _emailProviderFactory = new Mock<IEmailProviderFactory>();
        _dateTimeService = new Mock<IDateTimeService>();

        _emailSender = new(
            _templateService.Object,
            _notificationsService.Object,
            _emailProviderFactory.Object,
            _dateTimeService.Object
        );
    }

    [Fact]
    public async Task SendEmailAsync_WithInvalidTemplate_ShouldThrowException()
    {
        // Arrange
        SendEmailRequestDto request = CreateValidRequestDto();
        _templateService.Setup(x => 
            x.GetRuntimeTemplate(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<Language>(),
                It.IsAny<List<MetadataDto>>(),
                It.IsAny<string>(),
                It.IsAny<NotificationType>()
            )
        ).ThrowsAsync(new RuleValidationException("The template is not valid"));

        // Act & Assert
        await Assert.ThrowsAsync<RuleValidationException>(() => _emailSender.SendEmailAsync(request, request.Template.PlatformName));
    }

    [Fact]
    public async void SendEmailAsync_WithInvalidProvider_ShouldThrowException()
    {
        // Arrange
        SendEmailRequestDto request = CreateValidRequestDto();
        SetupValidRuntimeTemplate();

        _emailProviderFactory.Setup(
            x => x.CreateProviderAsync(
                It.IsAny<string>(),
                It.IsAny<string>()
            )
        ).ThrowsAsync(new RuleValidationException("Provider belongs to another platform"));

        // Act & Assert
        await Assert.ThrowsAsync<RuleValidationException>(() => _emailSender.SendEmailAsync(request, request.Template.PlatformName));
    }

    [Fact]
    public async Task SendEmailAsync_ShouldSaveAttachments_WhenMustSaveAttachmentsIsTrue()
    {
        // Arrange
        SendEmailRequestDto request = CreateValidRequestDto();
        SetupValidRuntimeTemplate();
        SetupSuccessNotificationResult();
        
        var attachments = CreateDummyAttachmentList(quantity: 1);

        // Act
        await _emailSender.SendEmailAsync(request, request.Template.PlatformName, attachments);
        
        // Assert
        _notificationsService
            .Verify(x => x.SaveAttachments(It.IsAny<IEnumerable<Attachment>>()), Times.Once);
    }

    [Fact]
    public async Task SendEmailAsync_ShouldNotSaveAttachments_WhenMustSaveAttachmentsIsFalse()
    {
        // Arrange
        SendEmailRequestDto request = CreateValidRequestDto();
        SetupValidRuntimeTemplate();
        SetupSuccessNotificationResult();

        var attachments = CreateDummyAttachmentList(quantity: 0);

        // Act
        await _emailSender.SendEmailAsync(request, request.Template.PlatformName, attachments);
        await _emailSender.SendEmailAsync(request, request.Template.PlatformName);
        
        // Assert
        _notificationsService
            .Verify(x => x.SaveAttachments(It.IsAny<IEnumerable<Attachment>>()), Times.Never);
    }

    [Fact]
    public async Task SendEmailAsync_WithFailedResult_ShouldReturnEssentialInformation()
    {
        // Arrange
        SendEmailRequestDto request = CreateValidRequestDto();
        SetupValidRuntimeTemplate();
        SetupFailedNotificationResult();

        // Act
        var actual = await _emailSender.SendEmailAsync(request, request.Template.PlatformName);

        // Assert
        Assert.False(actual.Response?.Success);
        Assert.NotNull(actual.Response?.Code);
        Assert.NotNull(actual.Response?.Message);
        Assert.NotNull(actual.Data?.NotificationId);
    }

    [Fact]
    public async Task SendEmailAsync_ShouldSendEmailSuccessfully()
    {
        // Arrange
        SendEmailRequestDto request = CreateValidRequestDto();

        SetupValidRuntimeTemplate();
        SetupSuccessNotificationResult();

        // Act
        var actual = await _emailSender.SendEmailAsync(request, request.Template.PlatformName);

        // Assert
        Assert.True(actual.Response?.Success);
        Assert.Equal((int) ResultCode.OK, actual.Response?.Code);
        Assert.NotNull(actual.Response?.Message);
        Assert.NotNull(actual.Data?.NotificationId);
    }

    [Fact]
    public async Task SendEmailAsync_WithSuccessResult_ShouldRegisterNotification()
    {
        // Arrange
        SendEmailRequestDto request = CreateValidRequestDto();
        SetupValidRuntimeTemplate();
        SetupSuccessNotificationResult();

        // Act
        var actual = await _emailSender.SendEmailAsync(request, request.Template.PlatformName);

        // Assert
        _notificationsService
            .Verify(x => x.RegisterNotification(It.IsAny<Notification>()), Times.Once);
    }

    [Fact]
    public async Task SendEmailAsync_WithFailedResult_ShouldRegisterNotification()
    {
        // Arrange
        SendEmailRequestDto request = CreateValidRequestDto();

        SetupValidRuntimeTemplate();
        SetupFailedNotificationResult();

        // Act
        var actual = await _emailSender.SendEmailAsync(request, request.Template.PlatformName);

        // Assert
        _notificationsService
            .Verify(x => x.RegisterNotification(It.IsAny<Notification>()), Times.Once);
    }

    #region Setup Methods
    private SendEmailRequestDto CreateValidRequestDto()
    {
        return new()
        {
            ToEmail = "johndoe@custom-domain.com",
            Template = new NotificationService.Core.Dtos.TemplateDto
            {
                Name = "MyCustomTemplate",
                PlatformName = "MyCustomPlatform",
                Language = Language.En,
                Metadata = new List<MetadataDto>()
            },
            ProviderName = "MyCustomProvider"
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

        _templateService.Setup(x => 
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

    private void SetupSuccessNotificationResult()
    {
        NotificationResult notificationResult 
            = NotificationResult.Ok(
                (int) ResultCode.OK,
                "Sent successfully",
                "no-reply@customer-service.com",
                savesAttachments: true);

        _emailProviderFactory
                .Setup(
                    f => f.CreateProviderAsync(
                        It.IsAny<string>(),
                        It.IsAny<string>()))
                .ReturnsAsync(Mock.Of<IEmailProvider>(
                    p => p.SendAsync(It.IsAny<EmailMessage>()) == Task.FromResult(notificationResult)));
    }

    private void SetupFailedNotificationResult()
    {
        NotificationResult notificationResult 
            = NotificationResult.Fail(
                (int) ResultCode.Error,
                "Sent failed");

        _emailProviderFactory
                .Setup(
                    f => f.CreateProviderAsync(
                        It.IsAny<string>(),
                        It.IsAny<string>()))
                .ReturnsAsync(Mock.Of<IEmailProvider>(
                    p => p.SendAsync(It.IsAny<EmailMessage>()) == Task.FromResult(notificationResult)));
    }

    private List<IFormFile> CreateDummyAttachmentList(int quantity = 0)
    {
        var attachments = new List<IFormFile>();
        
        for (var i=0; i < quantity; i++)
            attachments.Add(new Mock<IFormFile>().Object);

        return attachments;
    }
    #endregion
}