using System;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using NotificationService.Common.Entities;
using NotificationService.Contracts.ResponseDtos;
using NotificationService.Common.Dtos;
namespace NotificationService.Contracts.Interfaces.Services
{
    public interface ICatalogService
    {
        Task<BaseResponse<CatalogDto>> CreateCatalog(string name, string description, bool isActive, ICollection<ElementDto> elements, string owner);
        Task DeleteCatalog(string catalogId, string owner);
        Task<BaseResponse<IEnumerable<CatalogDto>>> GetCatalogs(Expression<Func<Catalog, bool>> filter, string owner, int? page, int? pageSize);
        Task<BaseResponse<CatalogDto>> GetCatalogById(string catalogId, string owner);
    }
}