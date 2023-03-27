using System;
using System.ComponentModel.DataAnnotations;
using NotificationService.Helpers;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using NotificationService.Repositories;
using NotificationService.Entities;
using NotificationService.Dtos;
using Microsoft.AspNetCore.Http;
namespace NotificationService.Attributes
{
    public class ValidateProvider : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var httpContextAccessor = (IHttpContextAccessor) validationContext.GetService(typeof(IHttpContextAccessor));

            var currentPlatform = (PlatformDTO) httpContextAccessor.HttpContext.Items[nameof(PlatformDTO)];
            var providerName = value as string;

            if (providerName != null)
            {
                // if (!(currentPlatform?.Providers?.Any(x => x == providerName) ?? false))
                    // return new ValidationResult($"Platform {currentPlatform.Name} does not have associated {providerName}");

            }
            return ValidationResult.Success;
        }
    }
}