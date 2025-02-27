using System.Linq.Expressions;
using NotificationService.Domain.Enums;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Models;
using NotificationService.Application.Contracts.DTOs.Requests;
using NotificationService.Application.Common.Dtos;
using NotificationService.Application.Common.Models;

namespace NotificationService.Application.Contracts.Services;

/// <summary>
/// Defines the contract for managing templates, including creation, retrieval, and updates.
/// </summary>
public interface ITemplateService
{
    /// <summary>
    /// Creates a new template.
    /// </summary>
    /// <param name="request">The request DTO containing the template's details.</param>
    /// <param name="owner">The owner of the template.</param>
    /// <returns>A <see cref="BaseResponse{TemplateDto}"/> containing the created template's details.</returns>
    Task<BaseResponse<TemplateDto>> CreateTemplate(CreateTemplateRequestDto request, string owner);

    /// <summary>
    /// Deletes a template by its unique identifier.
    /// </summary>
    /// <param name="templateId">The unique identifier of the template to delete.</param>
    /// <param name="owner">The owner of the template.</param>
    Task DeleteTemplate(string templateId, string owner);

    /// <summary>
    /// Retrieves a list of templates that match the specified filter criteria.
    /// </summary>
    /// <param name="filter">The filter expression to apply to the templates.</param>
    /// <param name="owner">The owner of the templates.</param>
    /// <param name="filterOptions">Additional filtering and pagination options.</param>
    /// <returns>A <see cref="BaseResponse{IEnumerable{TemplateDto}}"/> containing the matching templates.</returns>
    Task<BaseResponse<IEnumerable<TemplateDto>>> GetTemplates(Expression<Func<Template, bool>> filter, string owner, FilterOptions filterOptions);

    /// <summary>
    /// Retrieves a template by its unique identifier.
    /// </summary>
    /// <param name="templateId">The unique identifier of the template to retrieve.</param>
    /// <param name="owner">The owner of the template.</param>
    /// <returns>A <see cref="BaseResponse{TemplateDto}"/> containing the template's details.</returns>
    Task<BaseResponse<TemplateDto>> GetTemplateById(string templateId, string owner);

    /// <summary>
    /// Retrieves the final runtime template based on the specified parameters.
    /// This template is fully processed with all placeholders replaced by actual values, 
    /// making it ready for sending in notifications. 
    /// It ensures that the content is tailored to the specific requirements of the request.
    /// </summary>
    /// <param name="name">The name of the template.</param>
    /// <param name="platformName">The name of the platform associated with the template.</param>
    /// <param name="language">The language of the template.</param>
    /// <param name="providedMetadata">Metadata provided for the template in the request.</param>
    /// <param name="owner">The owner of the template.</param>
    /// <param name="notificationType">The type of notification for which the template is being retrieved.</param>
    /// <returns>A <see cref="RuntimeTemplate"/> representing the runtime version of the template.</returns>
    Task<RuntimeTemplate> GetRuntimeTemplate(string name, string platformName, Language language, List<Domain.Dtos.MetadataDto>? providedMetadata, string owner, NotificationType notificationType);

    /// <summary>
    /// Updates the content of an existing template.
    /// </summary>
    /// <param name="templateId">The unique identifier of the template to update.</param>
    /// <param name="request">The request DTO containing the updated content for the template.</param>
    /// <param name="owner">The owner of the template.</param>
    Task UpdateTemplateContent(string templateId, UpdateTemplateContentRequestDto request, string owner);
}