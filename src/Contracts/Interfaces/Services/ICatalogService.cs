using System;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using NotificationService.Core.Common.Entities;
using NotificationService.Contracts.ResponseDtos;
namespace NotificationService.Contracts.Interfaces.Services
{
    public interface ICatalogService
    {
        Task<FinalResponseDTO<CatalogDTO>> CreateCatalog(string name, string description, bool isActive, ICollection<ElementDTO> elements, string owner);
        Task DeleteCatalog(string catalogId, string owner);
        Task<FinalResponseDTO<IEnumerable<CatalogDTO>>> GetCatalogs(Expression<Func<Catalog, bool>> filter, string owner, int? page, int? pageSize);
        Task<FinalResponseDTO<CatalogDTO>> GetCatalogById(string catalogId, string owner);
    }
}