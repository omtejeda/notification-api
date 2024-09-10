using System.Globalization;
using Microsoft.AspNetCore.Http;
using NotificationService.Common.Entities;
using NotificationService.Common.Enums;
using NotificationService.Common.Exceptions;
using NotificationService.Common.Resources;

namespace NotificationService.Common.Utils
{
    public static class Guard
    {
        public static void ProviderIsNotNull(Provider provider, string providerName)
        {
            if (provider is null)
            {
                throw new ProviderException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Messages.ProviderSpecifiedNotExists,
                        providerName));
            }
        }

        public static void ProviderIsSuitable(ProviderType providerTypeSource, ProviderType providerTypeTarget)
        {
            if (providerTypeSource != providerTypeTarget)
            {
                throw new ProviderException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Messages.ProviderSpecifiedNotSuitable,
                        providerTypeSource));
            }
        }

        public static void ProviderIsCreatedByRequester(string createdBy, string requester)
        {
            if (createdBy != requester)
            {
                throw new ProviderException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Messages.ProviderWasNotCreatedByYou,
                        requester));
            }
        }

        public static void CatalogIsCreatedByRequester(string createdBy, string requester)
        {
            if (createdBy != requester)
            {
                throw new CatalogException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Messages.CatalogWasNotCreatedByYou,
                        requester));
            }
        }

        public static void CatalogWithIdExists(Catalog catalog, string catalogId)
        {
            if (catalog is null)
            {
                throw new CatalogException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Messages.CatalogWithGivenIdNotExists,
                        catalogId));
            }
        }

        public static void CatalogNotExists(Catalog catalog)
        {
            if (catalog is not null)
            {
                throw new CatalogException(
                    string.Format(
                        CultureInfo.CurrentCulture,
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
                    Messages.ProviderIsNotPublicNeitherWasCreatedByYou);
            }
        }

        public static void ProviderTypeIsValid(ProviderType providerType)
        {
            if (providerType == ProviderType.None)
            {
                throw new ProviderException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Messages.ProviderTypeNotValid,
                        providerType.ToString()));
            }
        }

        public static void ProviderNotExists(Provider provider)
        {
            if (provider is not null)
            {
                throw new ProviderException(
                    string.Format(
                        CultureInfo.CurrentCulture,
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
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Messages.ProviderWithGivenIdNotExists,
                        providerId));
            }
        }

        public static void CanSendToDestination(Provider provider, string toDestination, string? environment)
        {
            if (string.Equals(
                environment,
                EnvironmentConstants.ProductionName,
                StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var isDestinationAllowed = provider?
                .DevSettings?
                .AllowedRecipients?
                .Any(x => x == toDestination)
                ?? false;
            
            if (!isDestinationAllowed)
            {
                throw new RuleValidationException(
                    string.Format(
                        CultureInfo.CurrentCulture,
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
                    string.Format(
                        CultureInfo.CurrentCulture,
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
                    Messages.AttachmentIsRequired);
            }
        }

        public static void NotificationIsNotNull(object notification)
        {
            if (notification is null)
            {
                throw new NotificationException(
                    Messages.NotificationNotExists);
            }
        }

        public static void NotificationWasCreatedByRequester(string createdBy, string requester)
        {
            if (createdBy != requester)
            {
                throw new NotificationException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Messages.NotificationWasNotCreatedByYou,
                        requester));
            }
        }

        public static void NotificationRequestExists(object notificationRequest)
        {
            if (notificationRequest is null)
            {
                throw new NotificationException(
                    Messages.NotificationRequestNotFound);
            }
        }

        public static void NotificationTypeIsValid(NotificationType notificationType)
        {
            if (notificationType == NotificationType.None)
            {
                throw new NotificationException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Messages.NotificationTypeNotValid,
                        notificationType));
            }
        }

        public static void TemplateContentIsValid(string? content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new TemplateException(
                    Messages.TemplateContentNotValid);
            }
        }

        public static void TemplateIsNotNull()
        {
            
        }

        public static void TemplateNotExists(Template template)
        {
            if (template is not null)
            {
                throw new TemplateException(
                    Messages.TemplateAlreadyExists);
            }
        }

        public static void TemplateToDeleteExists(Template template)
        {
            if (template is null)
            {
                throw new TemplateException(
                    Messages.TemplateTryingToDeleteNotExists);
            }
        }

        public static void TemplateExists(Template template)
        {
            if (template is null)
            {
                throw new TemplateException(
                    Messages.TemplateNotExists);
            }
        }

        public static void TemplateBelongsToRequester(string createdBy, string requester)
        {
            if (createdBy != requester)
            {
                throw new TemplateException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Messages.TemplateWasNotCreatedByYou,
                        requester));
            }
        }

        public static void TemplateNotificationTypeIsSameAsTarget(NotificationType notificationTypeSource, NotificationType notificationTypeTarget)
        {
            if (notificationTypeSource != notificationTypeTarget)
            {
                throw new TemplateException(
                    string.Format(
                        CultureInfo.CurrentCulture,
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
                    Messages.TemplateWithNoContent);
            }
        }

        public static void CatalogExists(Catalog catalog, string catalogName)
        {
            if (catalog is null)
            {
                throw new CatalogException(
                    string.Format(
                        CultureInfo.CurrentCulture,
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
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Messages.CatalogSpecifiedNotHaveGivenKey,
                        catalog.Name,
                        key));
            }
        }

        public static void PlatformNotExists(Platform platform, string platformName, string createdBy)
        {
            if (platform is not null)
            {
                throw new PlatformException(
                    string.Format(
                        CultureInfo.CurrentCulture,
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
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Messages.PlatformWithGivenIdNotExists,
                        platformId));
            }
        }

        public static void PlatformIsCreatedByRequester(string createdBy, string requester)
        {
            if (createdBy != requester)
            {
                throw new PlatformException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Messages.PlatformWasNotCreatedByYou,
                        requester));
            }
        }

        public static void AttachmentExists(object? attachment, string fileName)
        {
            if (attachment is null)
            {
                throw new RuleValidationException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Messages.AttachmentNotFound,
                        fileName));
            }
        }

        public static void RequiredValueIsPresent(string? value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new RuleValidationException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Messages.RequiredValue,
                        parameterName));
            }
        }

        public static void RequiredValueIsPresent(int? value, string parameterName)
        {
            if (!value.HasValue)
            {
                throw new RuleValidationException(
                    string.Format(
                        CultureInfo.CurrentCulture,
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
                    Messages.RecipientAlreadyExists);
            }
        }

        public static void RecipientExists(string? recipient)
        {
            if (string.IsNullOrWhiteSpace(recipient))
            {
                throw new ProviderException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Messages.RecipientNotExists,
                        recipient));
            }
        }
    }
}