
using NotificationService.Application.Contracts.Interfaces.Services;
using NotificationService.Application.Providers.Factories.Interfaces;
using NotificationService.SharedKernel.Interfaces;
using NotificationService.Application.Senders;
using NotificationService.Domain.Enums;
using NotificationService.Application.Common.Dtos;
using NotificationService.Domain.Models;
using NotificationService.Domain.Entities;
using NotificationService.Application.Senders.Dtos;
using NotificationService.Application.Exceptions;
using NotificationService.Application.Providers.Interfaces;
using NotificationService.Application.Senders.Models;
using NotificationService.Application.Interfaces;
using Moq;
using Microsoft.AspNetCore.Http;

namespace NotificationService.Application.Tests.Senders;
public class EmailSenderTests
{
    private readonly Mock<ITemplateService> _templateServiceMock;
    private readonly Mock<INotificationsService> _notificationsServiceMock;
    private readonly Mock<IEmailProviderFactory> _emailProviderFactoryMock;
    private readonly Mock<IDateTimeService> _dateTimeServiceMock;
    private readonly IEmailSender _emailSender;

    public EmailSenderTests()
    {
        _templateServiceMock = new Mock<ITemplateService>();
        _notificationsServiceMock = new Mock<INotificationsService>();
        _emailProviderFactoryMock = new Mock<IEmailProviderFactory>();
        _dateTimeServiceMock = new Mock<IDateTimeService>();

        _emailSender = new EmailSender(
            _templateServiceMock.Object,
            _notificationsServiceMock.Object,
            _emailProviderFactoryMock.Object,
            _dateTimeServiceMock.Object
        );
    }

    [Fact]
    public async Task SendEmailAsync_WithInvalidTemplate_ShouldThrowException()
    {
        // Arrange
        SendEmailRequestDto request = CreateValidRequestDto();
        _templateServiceMock.Setup(x => 
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

        _emailProviderFactoryMock.Setup(
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
        _notificationsServiceMock
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
        _notificationsServiceMock
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
        _notificationsServiceMock
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
        _notificationsServiceMock
            .Verify(x => x.RegisterNotification(It.IsAny<Notification>()), Times.Once);
    }

    #region Setup Methods
    private SendEmailRequestDto CreateValidRequestDto()
    {
        return new()
        {
            ToEmail = "johndoe@custom-domain.com",
            Template = new NotificationService.Application.Senders.Dtos.TemplateDto
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

    private void SetupSuccessNotificationResult()
    {
        NotificationResult notificationResult 
            = NotificationResult.Ok(
                (int) ResultCode.OK,
                "Sent successfully",
                "no-reply@customer-service.com",
                savesAttachments: true);

        _emailProviderFactoryMock
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

        _emailProviderFactoryMock
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