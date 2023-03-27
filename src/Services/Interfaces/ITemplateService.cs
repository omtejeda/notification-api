using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using NotificationService.Entities;
using NotificationService.Dtos;
using NotificationService.Dtos.Requests;

namespace NotificationService.Services.Interfaces
{
    public interface ITemplateService
    {
        Task<FinalResponseDTO<TemplateDTO>> CreateTemplate(CreateTemplateRequestDto request, string owner);
        Task DeleteTemplate(string templateId, string owner);
        Task<FinalResponseDTO<IEnumerable<TemplateDTO>>> GetTemplates(Expression<Func<Template, bool>> filter, string owner, int? page, int? pageSize);
        Task<FinalResponseDTO<TemplateDTO>> GetTemplateById(string templateId, string owner);
    }
}