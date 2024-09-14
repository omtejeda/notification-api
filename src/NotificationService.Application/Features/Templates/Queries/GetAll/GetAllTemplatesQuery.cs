using NotificationService.Application.Common.Interfaces;
using NotificationService.Common.Dtos;

namespace NotificationService.Application.Features.Templates.Queries.GetAll;

public record GetAllTemplatesQuery : IQuery<BaseResponse<IEnumerable<TemplateDto>>>
{
    public string? Name { get; set; }
    public string? Subject { get; set; }
    public string? PlatformName { get; set; }
    public string? Owner { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}