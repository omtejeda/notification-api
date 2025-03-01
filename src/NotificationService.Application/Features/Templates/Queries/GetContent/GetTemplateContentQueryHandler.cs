using NotificationService.SharedKernel.Interfaces;
using NotificationService.Application.Contracts.Persistence;
using NotificationService.Domain.Entities;
using NotificationService.Application.Common.Dtos;
using NotificationService.Application.Common.Models;

namespace NotificationService.Application.Features.Templates.Queries.GetContent;

public class GetTemplateContentQueryHandler(IRepository<Template> templateRepository)
    : IQueryHandler<GetTemplateContentQuery, BaseResponse<TemplateContentDto>>
{
    private readonly IRepository<Template> _templateRepository = templateRepository;

    public async Task<BaseResponse<TemplateContentDto>> Handle(GetTemplateContentQuery request, CancellationToken cancellationToken)
    {
        var template = await _templateRepository
            .FindOneAsync(x => 
                x.TemplateId == request.TemplateId &&
                x.CreatedBy == request.Owner);

        if (template is null) return default!;
        
        var templateContent = new TemplateContentDto(template?.Content);
        return BaseResponse<TemplateContentDto>.Success(templateContent);

    }
}