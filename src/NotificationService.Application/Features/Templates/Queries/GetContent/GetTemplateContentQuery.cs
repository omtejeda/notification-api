using NotificationService.SharedKernel.Interfaces;
using NotificationService.Application.Common.Dtos;
using NotificationService.Application.Common.Models;

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