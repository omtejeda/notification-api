using NotificationService.Application.Common.Interfaces;
using NotificationService.Common.Dtos;
using NotificationService.Application.Contracts.Interfaces.Services;

namespace NotificationService.Application.Features.Templates.Queries.GetById;

public class GetTemplateByIdQueryHandler(ITemplateService templateService)
    : IQueryHandler<GetTemplateByIdQuery, BaseResponse<TemplateDto>>
{
    private readonly ITemplateService _templateService = templateService;

    public async Task<BaseResponse<TemplateDto>> Handle(GetTemplateByIdQuery request, CancellationToken cancellationToken)
    {
        return await _templateService.GetTemplateById(request.TemplateId!, request.Owner!);
    }
}