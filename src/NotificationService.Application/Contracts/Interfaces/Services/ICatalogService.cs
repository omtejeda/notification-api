using System.Linq.Expressions;
using NotificationService.Domain.Entities;
using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Application.Common.Models;

namespace NotificationService.Application.Contracts.Interfaces.Services;

public interface ICatalogService
{
    Task<BaseResponse<CatalogDto>> CreateCatalog(string name, string description, bool isActive, ICollection<ElementDto> elements, string owner);
    Task DeleteCatalog(string catalogId, string owner);
    Task<BaseResponse<IEnumerable<CatalogDto>>> GetCatalogs(Expression<Func<Catalog, bool>> filter, string owner, int? page, int? pageSize);
    Task<BaseResponse<CatalogDto>> GetCatalogById(string catalogId, string owner);
}