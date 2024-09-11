using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using NotificationService.Domain.Enums;
using NotificationService.Common.Dtos;
using NotificationService.Domain.Dtos;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Models;
using NotificationService.Contracts.RequestDtos;

namespace NotificationService.Contracts.Interfaces.Services
{
    public interface ITemplateService
    {
        Task<BaseResponse<TemplateDto>> CreateTemplate(CreateTemplateRequestDto request, string owner);
        Task DeleteTemplate(string templateId, string owner);
        Task<BaseResponse<IEnumerable<TemplateDto>>> GetTemplates(Expression<Func<Template, bool>> filter, string owner, int? page, int? pageSize);
        Task<BaseResponse<TemplateDto>> GetTemplateById(string templateId, string owner);
        Task<RuntimeTemplate> GetRuntimeTemplate(string name, string platformName, Language language, List<Domain.Dtos.MetadataDto> providedMetadata, string owner, NotificationType notificationType);
        Task UpdateTemplateContent(string templateId, UpdateTemplateContentRequestDto request, string owner);
    }
}