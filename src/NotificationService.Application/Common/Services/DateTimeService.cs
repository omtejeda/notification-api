using NotificationService.Common.Interfaces;

namespace NotificationService.Application.Common.Services;

public class DateTimeService : IDateTimeService
{
    private const int LocalTimeOffsetHours = -4;

    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
    public DateTime UtcToLocalTime => UtcNow.AddHours(LocalTimeOffsetHours);
}