using MediatR;

namespace NotificationService.SharedKernel.Interfaces;

public interface IEventHandler<TEvent> : INotificationHandler<TEvent> where TEvent : IEvent
{
}