using System;
using System.Linq;
using LinqKit;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Enums;
using NotificationService.Application.Contracts.Interfaces.Repositories;
using NotificationService.Application.Contracts.Interfaces.Services;
using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Common.Dtos;
using NotificationService.Common.Utils;

namespace NotificationService.Application.Catalogs.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Catalog> _catalogRepository;

        public CatalogService(IRepository<Catalog> catalogRepository, IMapper mapper)
        {
            _mapper = mapper;
            _catalogRepository = catalogRepository;
        }

        public async Task<BaseResponse<CatalogDto>> CreateCatalog(string name, string description, bool isActive, ICollection<ElementDto> elements, string owner)
        {
            var existingCatalog = await _catalogRepository.FindOneAsync(x => x.Name.ToLower() == name.ToLower() && x.CreatedBy == owner);

            Guard.CatalogNotExists(existingCatalog);
            
            var elementsEntity = _mapper.Map<ICollection<Element>>(elements);
            var catalog = new Catalog
            {
                CatalogId = Guid.NewGuid().ToString(),
                Name = name,
                Description = description,
                IsActive = isActive,
                Elements = elementsEntity,
                CreatedBy = owner ?? name
            };

            var entity = await _catalogRepository.InsertOneAsync(catalog);
            var catalogDto = _mapper.Map<CatalogDto>(entity);
            
            return BaseResponse<CatalogDto>.Success(catalogDto);
        }

        public async Task DeleteCatalog(string catalogId, string owner)
        {
            var existingCatalog = await _catalogRepository.FindOneAsync(x => x.CatalogId == catalogId);

            Guard.CatalogWithIdExists(existingCatalog, catalogId);
            Guard.CatalogIsCreatedByRequester(existingCatalog.CreatedBy, owner);

            await _catalogRepository.DeleteOneAsync(x => x.CatalogId == catalogId);
        }

        public async Task<BaseResponse<IEnumerable<CatalogDto>>> GetCatalogs(Expression<Func<Catalog, bool>> filter, string owner, int? page, int? pageSize)
        {
            var filterByOwner = PredicateBuilder.New<Catalog>().And(x => x.CreatedBy == owner).Expand();
            filter = filter.And(filterByOwner);

            var (catalogs, pagination) = await _catalogRepository.FindAsync(filter, page, pageSize);
            var catalogsDto = _mapper.Map<IEnumerable<CatalogDto>>(catalogs);
            var paginationDto = _mapper.Map<PaginationDto>(pagination);

            return BaseResponse<IEnumerable<CatalogDto>>.Success(catalogsDto, paginationDto);
        }

        public async Task<BaseResponse<CatalogDto>> GetCatalogById(string catalogId, string owner)
        {
            var catalog = await _catalogRepository.FindOneAsync(x => x.CatalogId == catalogId);
            if (catalog is null) return default!;

            Guard.CatalogIsCreatedByRequester(catalog.CreatedBy, owner);
            var catalogDto = _mapper.Map<CatalogDto>(catalog);

            return BaseResponse<CatalogDto>.Success(catalogDto);
        }
    }
}