namespace NotificationService.Application.Common.Interfaces;

public interface IDateTimeService
{
    DateTime Now { get; }
    DateTime UtcNow { get; }
    DateTime UtcToLocalTime { get; }
}