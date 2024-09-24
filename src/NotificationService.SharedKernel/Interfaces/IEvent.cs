using MediatR;

namespace NotificationService.SharedKernel.Interfaces;

/// <summary>
/// Represents an event that can be published through the MediatR library.
/// </summary>
public interface IEvent : INotification
{
}