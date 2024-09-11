using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using NotificationService.Domain.Enums;
using NotificationService.Common.Exceptions;
using NotificationService.Core.Common.Utils;
using LinqKit;
using NotificationService.Domain.Entities;
using NotificationService.Common.Dtos;
using NotificationService.Contracts.Interfaces.Services;
using NotificationService.Contracts.Interfaces.Repositories;
using NotificationService.Contracts.ResponseDtos;
using NotificationService.Common.Utils;
using NotificationService.Common.Resources;
using NotificationService.Common.Interfaces;

namespace NotificationService.Core.Platforms.Services
{
    public class PlatformService : IPlatformService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Platform> _repository;
        private readonly IEnvironmentService _environmentService;

        public PlatformService(
            IRepository<Platform> repository,
            IMapper mapper,
            IEnvironmentService environmentService)
        {
            _repository = repository;
            _mapper = mapper;
            _environmentService = environmentService;
        }

        public async Task<BaseResponse<PlatformDto>> CreatePlatform(string name, string description, string owner)
        {
            var existingPlatform = await _repository.FindOneAsync(x => x.Name.ToLower() == name.ToLower());

            Guard.PlatformNotExists(existingPlatform, name, existingPlatform.CreatedBy);
            var platform = new Platform
            {
                PlatformId = Guid.NewGuid().ToString(),
                Name = name,
                Description = description,
                IsActive = _environmentService.IsProduction == false,
                ApiKey = Guid.NewGuid().ToString(),
                CreatedBy = owner ?? name
            };

            var entity = await _repository.InsertOneAsync(platform);
            var platformDto = _mapper.Map<PlatformDto>(entity);

            return BaseResponse<PlatformDto>.Success(platformDto);
        }

        public async Task DeletePlatform(string platformId, string owner)
        {
            var existingPlatform = await _repository.FindOneAsync(x => x.PlatformId == platformId);
            
            Guard.PlatformWithIdExists(existingPlatform, platformId);
            Guard.PlatformIsCreatedByRequester(existingPlatform.CreatedBy, owner);

            await _repository.DeleteOneAsync(x => x.PlatformId == platformId);
        }

        public async Task<BaseResponse<IEnumerable<PlatformDto>>> GetPlatforms(Expression<Func<Platform, bool>> filter, string owner, int? page, int? pageSize)
        {
            var filterByOwner = PredicateBuilder.New<Platform>().And(x => x.CreatedBy == owner).Expand();
            filter = filter.And(filterByOwner);

            var (platforms, pagination) = await _repository.FindAsync(filter, page, pageSize);
            var platformsDto = _mapper.Map<IEnumerable<PlatformDto>>(platforms);
            var paginationDto = _mapper.Map<PaginationDto>(pagination);

            return BaseResponse<IEnumerable<PlatformDto>>.Success(platformsDto, paginationDto);
        }

        public async Task<BaseResponse<PlatformDto>> GetPlatformById(string platformId, string owner)
        {
            var platform = await _repository.FindOneAsync(x => x.PlatformId == platformId);
            if (platform is null) return default!;

            Guard.PlatformIsCreatedByRequester(platform.CreatedBy, owner);
            var platformDto = _mapper.Map<PlatformDto>(platform);

            return BaseResponse<PlatformDto>.Success(platformDto);
        }
    }
}