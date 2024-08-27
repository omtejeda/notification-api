using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using NotificationService.Common.Enums;
using NotificationService.Core.Common.Exceptions;
using NotificationService.Core.Common.Utils;
using LinqKit;
using NotificationService.Common.Entities;
using NotificationService.Common.Dtos;
using NotificationService.Contracts.Interfaces.Services;
using NotificationService.Contracts.Interfaces.Repositories;
using NotificationService.Contracts.ResponseDtos;
using NotificationService.Common.Utils;

namespace NotificationService.Core.Platforms.Services
{
    public class PlatformService : IPlatformService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Platform> _repository;
        private readonly IRepository<Provider> _providerRepository;

        public PlatformService(IRepository<Platform> repository, IRepository<Provider> providerRepository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _providerRepository = providerRepository;
        }

        public async Task<FinalResponseDto<PlatformDto>> CreatePlatform(string name, string description, string owner)
        {
            var existingPlatform = await _repository.FindOneAsync(x => x.Name.ToLower() == name.ToLower());

            if (existingPlatform is not null)
                throw new RuleValidationException($"There is already a platform named [{name}], created by {existingPlatform.CreatedBy}");

            var platform = new Platform
            {
                PlatformId = Guid.NewGuid().ToString(),
                Name = name,
                Description = description,
                IsActive = SystemUtil.IsProduction() == false,
                ApiKey = Guid.NewGuid().ToString(),
                CreatedBy = owner ?? name
            };

            var entity = await _repository.InsertOneAsync(platform);
            var platformDTO = _mapper.Map<PlatformDto>(entity);
            return new FinalResponseDto<PlatformDto>((int) ErrorCode.OK, platformDTO);
        }

        public async Task DeletePlatform(string platformId, string owner)
        {
            var existingPlatform = await _repository.FindOneAsync(x => x.PlatformId == platformId);

            if (existingPlatform is null)
                throw new RuleValidationException($"Does not exist a platform with ID [{platformId}]");

            if (existingPlatform.CreatedBy != owner)
                throw new RuleValidationException($"Platform was not created by {owner}");

            await _repository.DeleteOneAsync(x => x.PlatformId == platformId);
        }

        public async Task<FinalResponseDto<IEnumerable<PlatformDto>>> GetPlatforms(Expression<Func<Platform, bool>> filter, string owner, int? page, int? pageSize)
        {
            var filterByOwner = PredicateBuilder.New<Platform>().And(x => x.CreatedBy == owner).Expand();
            filter = filter.And(filterByOwner);

            var (platforms, pagination) = await _repository.FindAsync(filter, page, pageSize);
            var platformsDTO = _mapper.Map<IEnumerable<PlatformDto>>(platforms);
            var paginationDTO = _mapper.Map<PaginationDto>(pagination);

            return new FinalResponseDto<IEnumerable<PlatformDto>>( (int) ErrorCode.OK, platformsDTO, paginationDTO);
        }

        public async Task<FinalResponseDto<PlatformDto>> GetPlatformById(string platformId, string owner)
        {
            var platform = await _repository.FindOneAsync(x => x.PlatformId == platformId);

            if (platform is null) return default;

            if (platform.CreatedBy != owner)
                throw new RuleValidationException($"Platform was not created by {owner}");

            var platformDTO = _mapper.Map<PlatformDto>(platform);

            return new FinalResponseDto<PlatformDto>((int) ErrorCode.OK, platformDTO);
        }
    }
}