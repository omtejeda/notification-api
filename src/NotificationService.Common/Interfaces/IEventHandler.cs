using MediatR;

namespace NotificationService.Common.Interfaces;

public interface IEventHandler<TEvent> : INotificationHandler<TEvent> where TEvent : IEvent
{
}