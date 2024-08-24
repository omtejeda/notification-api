using System;
using System.Linq;
using LinqKit;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using NotificationService.Core.Common.Entities;
using NotificationService.Core.Common.Enums;
using NotificationService.Core.Common.Exceptions;
using NotificationService.Contracts.Interfaces.Repositories;
using NotificationService.Contracts.Interfaces.Services;
using NotificationService.Contracts.ResponseDtos;

namespace NotificationService.Core.Catalogs.Services
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

        public async Task<FinalResponseDTO<CatalogDTO>> CreateCatalog(string name, string description, bool isActive, ICollection<ElementDTO> elements, string owner)
        {
            var existingCatalog = await _catalogRepository.FindOneAsync(x => x.Name.ToLower() == name.ToLower() && x.CreatedBy == owner);

            if (existingCatalog is not null)
                throw new RuleValidationException($"There is already a catalog named [{name}], created by {(existingCatalog.CreatedBy == owner ? "You! :p" : existingCatalog.CreatedBy)}");
            
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
            var catalogDTO = _mapper.Map<CatalogDTO>(entity);
            return new FinalResponseDTO<CatalogDTO>((int) ErrorCode.OK, catalogDTO);
        }

        public async Task DeleteCatalog(string catalogId, string owner)
        {
            var existingCatalog = await _catalogRepository.FindOneAsync(x => x.CatalogId == catalogId);

            if (existingCatalog is null)
                throw new RuleValidationException($"Does not exist a catalog with ID [{catalogId}]");

            if (existingCatalog.CreatedBy != owner)
                throw new RuleValidationException($"Catalog was not created by {owner}");

            await _catalogRepository.DeleteOneAsync(x => x.CatalogId == catalogId);
        }

        public async Task<FinalResponseDTO<IEnumerable<CatalogDTO>>> GetCatalogs(Expression<Func<Catalog, bool>> filter, string owner, int? page, int? pageSize)
        {
            var filterByOwner = PredicateBuilder.New<Catalog>().And(x => x.CreatedBy == owner).Expand();
            filter = filter.And(filterByOwner);

            var (catalogs, pagination) = await _catalogRepository.FindAsync(filter, page, pageSize);
            var catalogsDTO = _mapper.Map<IEnumerable<CatalogDTO>>(catalogs);
            var paginationDTO = _mapper.Map<PaginationDTO>(pagination);

            return new FinalResponseDTO<IEnumerable<CatalogDTO>>( (int) ErrorCode.OK, catalogsDTO, paginationDTO);
        }

        public async Task<FinalResponseDTO<CatalogDTO>> GetCatalogById(string catalogId, string owner)
        {
            var catalog = await _catalogRepository.FindOneAsync(x => x.CatalogId == catalogId);

            if (catalog is null) return default;

            if (catalog.CreatedBy != owner)
                throw new RuleValidationException($"Catalog was not created by {owner}");

            var catalogDTO = _mapper.Map<CatalogDTO>(catalog);

            return new FinalResponseDTO<CatalogDTO>((int) ErrorCode.OK, catalogDTO);
        }
    }
}