using NotificationService.Application.Common.Interfaces;
using NotificationService.Common.Dtos;

namespace NotificationService.Application.Features.Templates.Queries.GetById;

public record GetTemplateByIdQuery : IQuery<BaseResponse<TemplateDto>>
{
    public GetTemplateByIdQuery(string? templateId, string? owner)
    {
        TemplateId = templateId;
        Owner = owner;
    }
    
    public string? TemplateId { get; set; }
    public string? Owner { get; set; }
}