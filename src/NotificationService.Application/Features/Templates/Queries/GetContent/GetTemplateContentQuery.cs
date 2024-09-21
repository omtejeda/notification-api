using NotificationService.Common.Interfaces;
using NotificationService.Application.Common.Dtos;

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