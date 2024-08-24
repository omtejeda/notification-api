using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using NotificationService.Core.Common.Enums;
using NotificationService.Core.Templates.Dtos;
using NotificationService.Core.Templates.Entities;
using NotificationService.Core.Templates.Models;
using NotificationService.Contracts.RequestDtos;
using NotificationService.Contracts.ResponseDtos;

namespace NotificationService.Contracts.Interfaces.Services
{
    public interface ITemplateService
    {
        Task<FinalResponseDTO<TemplateDTO>> CreateTemplate(CreateTemplateRequestDto request, string owner);
        Task DeleteTemplate(string templateId, string owner);
        Task<FinalResponseDTO<IEnumerable<TemplateDTO>>> GetTemplates(Expression<Func<Template, bool>> filter, string owner, int? page, int? pageSize);
        Task<FinalResponseDTO<TemplateDTO>> GetTemplateById(string templateId, string owner);
        Task<RuntimeTemplate> GetRuntimeTemplate(string name, string platformName, Language language, List<MetadataDto> providedMetadata, string owner, NotificationType notificationType);
        Task UpdateTemplateContent(string templateId, UpdateTemplateContentRequestDto request, string owner);
    }
}