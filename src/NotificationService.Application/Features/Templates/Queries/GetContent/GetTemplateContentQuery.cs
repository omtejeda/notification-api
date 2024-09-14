using NotificationService.Application.Common.Interfaces;
using NotificationService.Common.Dtos;
using NotificationService.Domain.Entities;

namespace NotificationService.Application.Features.Templates.Queries.GetContent;

public record GetTemplateContentQuery : IQuery<BaseResponse<TemplateContentDto>>
{
    public GetTemplateContentQuery(string? templateId, string? owner)
    {
        TemplateId = templateId;
        Owner = owner;
    }
    
    public string? TemplateId { get; set; }
    public string? Owner { get; set; }
}