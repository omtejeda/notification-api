using MediatR;

namespace NotificationService.SharedKernel.Interfaces;

/// <summary>
/// Defines a handler for processing events of type <typeparamref name="TEvent"/>.
/// </summary>
/// <typeparam name="TEvent">The type of event that this handler processes.</typeparam>
public interface IEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : IEvent
{
}