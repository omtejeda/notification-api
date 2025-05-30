using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using NotificationService.Domain.Enums;
using NotificationService.Application.Common.Dtos;
using NotificationService.Application.Exceptions;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Models;
using NotificationService.Application.Contracts.DTOs.Requests;
using NotificationService.Application.Contracts.Persistence;
using NotificationService.Application.Contracts.Services;
using Moq;
using AutoMapper;
using NotificationService.Application.Features.Templates.Services;
using NotificationService.Application.Common.Models;

namespace NotificationService.Application.Tests.Services;

public class TemplateServiceTests
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRepository<Template>> _templateRepositoryMock;
    private readonly Mock<IRepository<Catalog>> _catalogRepositoryMock;
    private readonly ITemplateService _templateService;

    public TemplateServiceTests()
    {
        _mapperMock = new Mock<IMapper>();
        _templateRepositoryMock = new Mock<IRepository<Template>>();
        _catalogRepositoryMock = new Mock<IRepository<Catalog>>();

        _templateService = new TemplateService(
            _templateRepositoryMock.Object,
            _catalogRepositoryMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task CreateTemplate_ShouldThrowException_WhenTemplateExists()
    {
        // Arrange
        CreateTemplateRequestDto request = CreateTemplate();
        
        List<Template> templatesFound = [
            new Template
            {
                Name = "ExistingTemplate",
                Language = Language.En
            }
        ];

        _templateRepositoryMock
            .Setup(x => x.FindAsync(
                It.IsAny<Expression<Func<Template, bool>>>(),
                It.IsAny<FilterOptions>()
            ))
            .ReturnsAsync((templatesFound, default));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<TemplateException>(() => _templateService.CreateTemplate(request, "owner"));
        var expectedMessage = "Template already exists";

        Assert.Equal(expectedMessage, exception.Message);
    }

    #region TestData
    private CreateTemplateRequestDto CreateTemplate()
    {
        return new CreateTemplateRequestDto
        {
            Name = "MyCustomTemplate",
            Language = Language.En,
            NotificationType = "Email",
            Subject = "Welcome to our platform",
            Content = "Hi there!",
            Metadata = new List<MetadataRequired>()
            {
                new MetadataRequired
                {
                    Key = "ClientName",
                    Description = "The name of the client being addressed",
                    IsRequired = true
                }
            },
            Labels = new List<TemplateLabelDto>()
        };
    }
    #endregion
}