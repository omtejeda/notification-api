using System.Linq.Expressions;
using NotificationService.Domain.Enums;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Models;
using NotificationService.Application.Contracts.RequestDtos;
using NotificationService.Application.Common.Dtos;
using NotificationService.Application.Common.Models;

namespace NotificationService.Application.Contracts.Interfaces.Services;

public interface ITemplateService
{
    Task<BaseResponse<TemplateDto>> CreateTemplate(CreateTemplateRequestDto request, string owner);
    Task DeleteTemplate(string templateId, string owner);
    Task<BaseResponse<IEnumerable<TemplateDto>>> GetTemplates(Expression<Func<Template, bool>> filter, string owner, FilterOptions filterOptions);
    Task<BaseResponse<TemplateDto>> GetTemplateById(string templateId, string owner);
    Task<RuntimeTemplate> GetRuntimeTemplate(string name, string platformName, Language language, List<Domain.Dtos.MetadataDto>? providedMetadata, string owner, NotificationType notificationType);
    Task UpdateTemplateContent(string templateId, UpdateTemplateContentRequestDto request, string owner);
}