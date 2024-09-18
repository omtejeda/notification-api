using System.Globalization;
using Microsoft.AspNetCore.Http;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Enums;
using NotificationService.Application.Exceptions;
using NotificationService.Common.Resources;
using NotificationService.Common.Utils;
using System.Diagnostics.CodeAnalysis;

namespace NotificationService.Application.Utils;

public static class Guard
{
    public static void ProviderIsNotNull(Provider provider, string providerName)
    {
        if (provider is null)
        {
            throw new ProviderException(
                Format(
                    Messages.ProviderSpecifiedNotExists,
                    providerName));
        }
    }

    public static void ProviderIsSuitable(ProviderType providerTypeSource, ProviderType providerTypeTarget)
    {
        if (providerTypeSource != providerTypeTarget)
        {
            throw new ProviderException(
                Format(
                    Messages.ProviderSpecifiedNotSuitable,
                    providerTypeSource));
        }
    }

    public static void ProviderIsCreatedByRequester(string? createdBy, string requester)
    {
        if (createdBy != requester)
        {
            throw new ProviderException(
                Format(
                    Messages.ProviderWasNotCreatedByYou,
                    requester));
        }
    }

    public static void CatalogIsCreatedByRequester(string? createdBy, string requester)
    {
        if (createdBy != requester)
        {
            throw new CatalogException(
                Format(
                    Messages.CatalogWasNotCreatedByYou,
                    requester));
        }
    }

    public static void CatalogWithIdExists(Catalog catalog, string catalogId)
    {
        if (catalog is null)
        {
            throw new CatalogException(
                Format(
                    Messages.CatalogWithGivenIdNotExists,
                    catalogId));
        }
    }

    public static void CatalogNotExists(Catalog catalog)
    {
        if (catalog is not null)
        {
            throw new CatalogException(
                Format(
                    Messages.CatalogAlreadyExists,
                    catalog.Name,
                    catalog.CreatedBy));
        }
    }

    public static void ProviderIsCreatedByRequesterOrPublic(Provider provider, string requester)
    {
        if (!(provider.IsPublic ?? false) && provider.CreatedBy != requester)
        {
            throw new ProviderException(
                Format(
                    Messages.ProviderIsNotPublicNeitherWasCreatedByYou));
        }
    }

    public static void ProviderTypeIsValid(ProviderType providerType)
    {
        if (providerType == ProviderType.None)
        {
            throw new ProviderException(
                Format(
                    Messages.ProviderTypeNotValid,
                    providerType.ToString()));
        }
    }

    public static void ProviderNotExists(Provider provider)
    {
        if (provider is not null)
        {
            throw new ProviderException(
                Format(
                    Messages.ProviderAlreadyExists,
                    provider.Name,
                    provider.CreatedBy));
        }
    }

    public static void ProviderWithIdExists(Provider provider, string providerId)
    {
        if (provider is null)
        {
            throw new ProviderException(
                Format(
                    Messages.ProviderWithGivenIdNotExists,
                    providerId));
        }
    }

    public static void CanSendToDestination(Provider provider, string? toDestination, string? environment)
    {
        if (string.Equals(
            environment,
            EnvironmentConstants.ProductionName,
            StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        toDestination ??= string.Empty;

        var isDestinationAllowed = provider?
            .DevSettings?
            .AllowedRecipients?
            .Any(x => x == toDestination)
            ?? false;
        
        if (!isDestinationAllowed)
        {
            throw new RuleValidationException(
                Format(
                    Messages.NotAllowedToSendInNonProd,
                    toDestination));
        }
    }

    public static void NotificationTypeIsValidForBasicMessage(NotificationType notificationTypeSource)
    {
        if (notificationTypeSource == NotificationType.Email || 
            notificationTypeSource == NotificationType.SMS)
        {
            throw new NotificationException(
                Format(
                    Messages.NotificationTypeSpecifiedNotAllowed,
                    notificationTypeSource));
        }
    }

    public static void EmailProviderIsNotNull(object? emailProvider)
    {
        if (emailProvider is null)
        {
            throw new ProviderException(
                "Underlying provider could not be found");
        }
    }

    public static void ProviderNameAndCreatedByHasValue(string providerName, string createdBy)
    {
        if (string.IsNullOrWhiteSpace(providerName))
        {
            throw new ArgumentNullException(
                nameof(providerName));
        }
        
        if (string.IsNullOrWhiteSpace(createdBy))
        {
            throw new ArgumentNullException(
                nameof(createdBy));
        }
    }

    public static void HasAttachments(List<IFormFile> attachments)
    {
        if (!attachments.Any())
        {
            throw new RuleValidationException(
                Format(Messages.AttachmentIsRequired));
        }
    }

    public static void NotificationIsNotNull(object notification)
    {
        if (notification is null)
        {
            throw new NotificationException(
                Format(Messages.NotificationNotExists));
        }
    }

    public static void NotificationWasCreatedByRequester(string? createdBy, string requester)
    {
        if (createdBy != requester)
        {
            throw new NotificationException(
                Format(
                    Messages.NotificationWasNotCreatedByYou,
                    requester));
        }
    }

    public static void NotificationRequestExists([NotNull] object? notificationRequest)
    {
        if (notificationRequest is null)
        {
            throw new NotificationException(
                Format(Messages.NotificationRequestNotFound));
        }
    }

    public static void NotificationTypeIsValid(NotificationType notificationType)
    {
        if (notificationType == NotificationType.None)
        {
            throw new NotificationException(
                Format(
                    Messages.NotificationTypeNotValid,
                    notificationType));
        }
    }

    public static void TemplateContentIsValid(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new TemplateException(
                Format(Messages.TemplateContentNotValid));
        }
    }

    public static void TemplateNotExists(Template? template)
    {
        if (template is not null)
        {
            throw new TemplateException(
                Format(Messages.TemplateAlreadyExists));
        }
    }

    public static void TemplateToDeleteExists(Template template)
    {
        if (template is null)
        {
            throw new TemplateException(
                Format(Messages.TemplateTryingToDeleteNotExists));
        }
    }

    public static void TemplateExists(Template? template)
    {
        if (template is null)
        {
            throw new TemplateException(
                Format(Messages.TemplateNotExists));
        }
    }

    public static void TemplateBelongsToRequester(string? createdBy, string requester)
    {
        if (createdBy != requester)
        {
            throw new TemplateException(
                Format(
                    Messages.TemplateWasNotCreatedByYou,
                    requester));
        }
    }

    public static void TemplateNotificationTypeIsSameAsTarget(NotificationType notificationTypeSource, NotificationType? notificationTypeTarget)
    {
        if (notificationTypeSource != notificationTypeTarget)
        {
            throw new TemplateException(
                Format(
                    Messages.TemplateSpecifiedNotCorrespondToGivenNotificationType,
                    notificationTypeSource,
                    notificationTypeTarget));
        }
            
    }

    public static void TemplateHasContent(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new TemplateException(
                Format(Messages.TemplateWithNoContent));
        }
    }

    public static void CatalogExists(Catalog catalog, string catalogName)
    {
        if (catalog is null)
        {
            throw new CatalogException(
                Format(
                    Messages.CatalogSpecifiedNotExists,
                    catalogName));
        }
    }

    public static void CatalogHasKey(Catalog catalog, string catalogName, string key)
    {
        CatalogExists(catalog, catalogName);
        
        if (!catalog.Elements.Any(x => x.Key == key))
        {
            throw new CatalogException(
                Format(
                    Messages.CatalogSpecifiedNotHaveGivenKey,
                    catalog.Name,
                    key));
        }
    }

    public static void PlatformNotExists(Platform platform, string platformName, string? createdBy)
    {
        if (platform is not null)
        {
            throw new PlatformException(
                Format(
                    Messages.PlatformAlreadyExists,
                    platformName,
                    createdBy));
        }
    }

    public static void PlatformWithIdExists(Platform platform, string platformId)
    {
        if (platform is null)
        {
            throw new PlatformException(
                Format(
                    Messages.PlatformWithGivenIdNotExists,
                    platformId));
        }
    }

    public static void PlatformIsCreatedByRequester(string? createdBy, string requester)
    {
        if (createdBy != requester)
        {
            throw new PlatformException(
                Format(
                    Messages.PlatformWasNotCreatedByYou,
                    requester));
        }
    }

    public static void AttachmentExists(object? attachment, string fileName)
    {
        if (attachment is null)
        {
            throw new RuleValidationException(
                Format(
                    Messages.AttachmentNotFound,
                    fileName));
        }
    }

    public static void RequiredValueIsPresent(string? value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new RuleValidationException(
                Format(
                    Messages.RequiredValue,
                    parameterName));
        }
    }

    public static void RequiredValueIsPresent(int? value, string parameterName)
    {
        if (!value.HasValue)
        {
            throw new RuleValidationException(
                Format(
                    Messages.RequiredValue,
                    parameterName));
        }
    }

    public static void RecipientNotExists(Provider provider, string recipient)
    {
        var recipientExists = provider
            .DevSettings
            .AllowedRecipients
            .Any(x => x.ToLower() == recipient.ToLower());
        
        if (recipientExists)
        {
            throw new ProviderException(
                Format(Messages.RecipientAlreadyExists));
        }
    }

    public static void RecipientExists([NotNull] string? recipient)
    {
        if (string.IsNullOrWhiteSpace(recipient))
        {
            throw new ProviderException(
                Format(
                    Messages.RecipientNotExists,
                    recipient));
        }
    }

    private static string Format(string format, params object?[] args)
        => string.Format(CultureInfo.CurrentCulture, format, args);
}