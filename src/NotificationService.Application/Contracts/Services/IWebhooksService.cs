namespace NotificationService.Application.Contracts.Services;

/// <summary>
/// Provides methods for handling webhooks related to notifications.
/// </summary>
public interface IWebhooksService
{
    /// <summary>
    /// Saves the content of an email sent.
    /// This method is used in specific cases when sending notifications with a SendGrid Provider
    /// and the template as well as its content is managed directly in SendGrid.
    /// In order to know the final values that SendGrid sent, a callback to this method is required.
    /// It allows for further processing or tracking of sent notifications.
    /// </summary>
    /// <param name="content">The body content of the email.</param>
    /// <param name="subject">The subject line of the email.</param>
    /// <param name="headers">The headers associated with the email.</param>
    /// <returns>A tuple containing a boolean indicating success or failure, and a string for additional information.</returns>
    Task<(bool, string)> SaveEmailContent(string content, string subject, string headers);
}