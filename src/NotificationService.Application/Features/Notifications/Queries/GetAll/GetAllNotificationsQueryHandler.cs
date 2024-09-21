using NotificationService.SharedKernel.Interfaces;
using NotificationService.Application.Contracts.Interfaces.Services;
using NotificationService.Domain.Entities;
using LinqKit;
using System.Linq.Expressions;
using NotificationService.Application.Contracts.ResponseDtos;
using System.Diagnostics.CodeAnalysis;
using NotificationService.Application.Utils;
using NotificationService.Application.Common.Models;

namespace NotificationService.Application.Features.Notifications.Queries.GetAll;

public class GetAllNotificationsQueryHandler(INotificationsService notificationsService)
    : IQueryHandler<GetAllNotificationsQuery, BaseResponse<IEnumerable<NotificationDto>>>
{
    private readonly INotificationsService _notificationsService = notificationsService;

    public async Task<BaseResponse<IEnumerable<NotificationDto>>> Handle(GetAllNotificationsQuery request, CancellationToken cancellationToken)
    {
        var predicate = GetPredicateExpression(request);
        return await _notificationsService.GetNotifications(predicate, request.GetOwner()!, new FilterOptions(request.Page, request.PageSize, request.Sort));
    }

    private static Expression<Func<Notification, bool>> GetPredicateExpression(GetAllNotificationsQuery query)
    {
        var predicate = PredicateBuilder.New<Notification>(true);

        if (!IsBlank(query.NotificationId))
            predicate = predicate.And(x => x.NotificationId == query.NotificationId);
            
        if (!IsBlank(query.ToDestination))
            predicate = predicate.And(x => x.ToDestination == query.ToDestination);
        
        if (!IsBlank(query.TemplateName))
            predicate = predicate.And(x => x.TemplateName == query.TemplateName);
        
        if (!IsBlank(query.PlatformName))
            predicate = predicate.And(x => x.PlatformName == query.PlatformName);
        
        if (!IsBlank(query.ProviderName))
            predicate = predicate.And(x => x.ProviderName == query.ProviderName);
        
        if (!IsBlank(query.Subject))
            predicate = predicate.And(x => x.Subject.Contains(query.Subject, StringComparison.CurrentCultureIgnoreCase));
        
        if (query.Success.HasValue)
            predicate = predicate.And(x => x.Success == query.Success);
        
        if (query.HasAttachments.HasValue)
            predicate = predicate.And(x => x.HasAttachments == query.HasAttachments);
        
        if (query.HasParentNotification == true)
            predicate = predicate.And(x => x.ParentNotificationId != null);
        
        if (query.HasParentNotification == false)
            predicate = predicate.And(x => x.ParentNotificationId == null);

        return predicate;
    }

    private static bool IsBlank([NotNullWhen(false)] string? value)
        => string.IsNullOrWhiteSpace(value);
}