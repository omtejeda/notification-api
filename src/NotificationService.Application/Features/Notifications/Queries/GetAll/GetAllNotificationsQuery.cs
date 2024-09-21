using NotificationService.SharedKernel.Interfaces;
using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Application.Common.Dtos;

namespace NotificationService.Application.Features.Notifications.Queries.GetAll;

public record GetAllNotificationsQuery : OwnedQuery, IQuery<BaseResponse<IEnumerable<NotificationDto>>>
{
    public string? NotificationId { get; set; }
    public string? ToDestination { get; set; }
    public string? TemplateName { get; set; }
    public string? PlatformName { get; set; }
    public string? ProviderName { get; set; }
    public string? Subject { get; set; }
    public bool? Success { get; set; }
    public bool? HasAttachments { get; set; }
    public bool? HasParentNotification { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
    public string? Sort { get; set; }
}