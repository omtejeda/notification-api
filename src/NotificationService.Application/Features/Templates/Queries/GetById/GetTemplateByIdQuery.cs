using NotificationService.Application.Common.Dtos;
using NotificationService.Application.Common.Models;
using NotificationService.SharedKernel.Interfaces;

namespace NotificationService.Application.Features.Templates.Queries.GetById;

public record GetTemplateByIdQuery : IQuery<BaseResponse<TemplateDto>>
{
    public GetTemplateByIdQuery(string templateId, string owner)
    {
        TemplateId = templateId;
        Owner = owner;
    }
    
    public string TemplateId { get; set; }
    public string Owner { get; set; }
}