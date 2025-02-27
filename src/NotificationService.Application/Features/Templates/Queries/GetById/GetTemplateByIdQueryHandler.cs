using NotificationService.SharedKernel.Interfaces;
using NotificationService.Application.Contracts.Services;
using NotificationService.Application.Common.Dtos;
using NotificationService.Application.Common.Models;

namespace NotificationService.Application.Features.Templates.Queries.GetById;

public class GetTemplateByIdQueryHandler(ITemplateService templateService)
    : IQueryHandler<GetTemplateByIdQuery, BaseResponse<TemplateDto>>
{
    private readonly ITemplateService _templateService = templateService;

    public async Task<BaseResponse<TemplateDto>> Handle(GetTemplateByIdQuery request, CancellationToken cancellationToken)
    {
        return await _templateService.GetTemplateById(request.TemplateId, request.Owner);
    }
}