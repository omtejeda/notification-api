using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NotificationService.Common.Entities;
using NotificationService.Contracts.Interfaces.Repositories;
using NotificationService.Core.Dtos;
namespace NotificationService.Core.Templates.Attributes
{
    public class ValidateTemplate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var templateRepository = (IRepository<Template>) validationContext.GetService(typeof(IRepository<Template>));
            var template = value as TemplateDto;

            if (template is not null)
            {
                var templatesFound = templateRepository.Find(x => x.Name == template.Name && x.PlatformName == template.PlatformName);

                if (!templatesFound.Any())
                    return new ValidationResult($"Template [{template.Name}] for platform [{template.PlatformName}] is not valid");

                var originalTemplate = templatesFound.Where(x => x.Language == template.Language).FirstOrDefault();

                if (originalTemplate is null)
                    return new ValidationResult($"Do not exist template for the language [{template.Language}]");

                var keysRequired = originalTemplate.Metadata.Where(x => x.IsRequired);
                var keysProvided = template.Metadata;

                var result = keysRequired.Where(x => !keysProvided.Any(y => y.Key == x.Key && !string.IsNullOrWhiteSpace(y.Value))).Select(x => x.Key);

                if (result.Any())
                {
                    var missingKeys = string.Join(", ", result);
                    return new ValidationResult($"Metadata provided not valid. These metadata need to be provided: {missingKeys}");
                }

            }
            return ValidationResult.Success;
        }
    }
}