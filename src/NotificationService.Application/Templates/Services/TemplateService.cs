using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Enums;
using LinqKit;
using NotificationService.Application.Utils;
using NotificationService.Domain.Dtos;
using NotificationService.Common.Dtos;
using NotificationService.Domain.Models;
using NotificationService.Application.Contracts.RequestDtos;
using NotificationService.Application.Contracts.Interfaces.Services;
using NotificationService.Application.Contracts.Interfaces.Repositories;

namespace NotificationService.Application.Templates.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Template> _repository;
        private readonly IRepository<Catalog> _catalogRepository;

        public TemplateService(IRepository<Template> repository, IRepository<Catalog> catalogRepository, IMapper mapper)
        {
            _repository = repository;
            _catalogRepository = catalogRepository;
            _mapper = mapper;
        }
        public async Task<BaseResponse<TemplateDto>> CreateTemplate(CreateTemplateRequestDto request, string owner)
        {
            Enum.TryParse(request.NotificationType, out NotificationType notificationType);
            Guard.NotificationTypeIsValid(notificationType);

            var existingTemplate = await FindByCompositeKeyAsync(request.Name, platformName: owner, request.Language, owner);
            
            Guard.TemplateNotExists(existingTemplate);

            var metadata = request.Metadata.Select(x => new Domain.Models.Metadata { Key = x.Key, Description = x.Description, IsRequired = x.IsRequired }).ToList();
            var labels = _mapper.Map<ICollection<TemplateLabel>>(request.Labels) ?? Array.Empty<TemplateLabel>();


            foreach (var label in labels.Where(x => !string.IsNullOrWhiteSpace(x.CatalogNameToCheckAgainst)))
            {
                var catalog = await _catalogRepository.FindOneAsync(x => x.Name == label.CatalogNameToCheckAgainst);
                
                Guard.CatalogExists(catalog, label.CatalogNameToCheckAgainst);
                Guard.CatalogHasKey(catalog, catalog.Name, key: label.Value);
            }

            var template = new Template
            {
                TemplateId = Guid.NewGuid().ToString(),
                Name = request.Name,
                PlatformName = owner,
                Language = request.Language,
                NotificationType = notificationType,
                Subject = request.Subject,
                Content = request.Content,
                Metadata = metadata,
                Labels = labels,
                CreatedBy = owner
            };

            var entity = await _repository.InsertOneAsync(template);
            var templateDto = _mapper.Map<TemplateDto>(entity);
            
            return BaseResponse<TemplateDto>.Success(templateDto);
        }
        public async Task DeleteTemplate(string templateId, string owner)
        {
            var existingTemplate = await _repository.FindOneAsync(x => x.TemplateId == templateId);

            Guard.TemplateBelongsToRequester(existingTemplate.CreatedBy, owner);
            Guard.TemplateToDeleteExists(existingTemplate);
            
            await _repository.DeleteOneAsync(x => x.Id == existingTemplate.Id);
        }

        private async Task<Template> FindByCompositeKeyAsync(string name, string platformName, Language language, string owner)
        {
            var (templates, _) = await _repository.FindAsync(x => x.Name == name && x.PlatformName == platformName && x.CreatedBy == owner);
            var template = templates.FirstOrDefault(x => x.Language == language);

            return template;
        }

        public async Task<BaseResponse<IEnumerable<TemplateDto>>> GetTemplates(Expression<Func<Template, bool>> filter, string owner, int? page, int? pageSize)
        {
            var filterByOwner = PredicateBuilder.New<Template>().And(x => x.CreatedBy == owner).Expand();
            filter = filter.And(filterByOwner);
            
            var (templates, pagination) = await _repository.FindAsync(filter, page, pageSize);
            var templatesDto = _mapper.Map<IEnumerable<TemplateDto>>(templates);
            var paginationDto = _mapper.Map<PaginationDto>(pagination);

            return BaseResponse<IEnumerable<TemplateDto>>.Success(templatesDto, paginationDto);
        }

        public async Task<BaseResponse<TemplateDto>> GetTemplateById(string templateId, string owner)
        {
            var template = await _repository.FindOneAsync(x => x.TemplateId == templateId);
            if (template is null) return default!;
            
            Guard.TemplateBelongsToRequester(template.CreatedBy, owner);
            var templateDto = _mapper.Map<TemplateDto>(template);

            return BaseResponse<TemplateDto>.Success(templateDto);
        }

        public async Task<RuntimeTemplate> GetRuntimeTemplate(string name, string platformName, Language language, List<Domain.Dtos.MetadataDto> providedMetadata, string owner, NotificationType notificationType)
        {
            var (templates, _) = await _repository
                .FindAsync(t => t.Name == name && t.PlatformName == platformName);
            
            var template = templates
                .FirstOrDefault(x => x.Language == language);
            
            ThrowIfTemplateNotValid(template, owner, notificationType);

            var providedTemplateMetadata = providedMetadata
                .Where(x => template.Metadata.Any(y => y.Key == x.Key));
            
            var runtimeTemplate = new RuntimeTemplate
            {
                Name = template.Name,
                PlatformName = template.PlatformName,
                Language = template.Language,
                ProvidedMetadata = providedMetadata,
                Subject = EmailUtil.ReplaceParameters(template.Subject, providedTemplateMetadata),
                Content = EmailUtil.ReplaceParameters(template.Content, providedTemplateMetadata)
            };

            return runtimeTemplate;
        }

        private void ThrowIfTemplateNotValid(Template template, string owner, NotificationType notificationType)
        {
            Guard.TemplateExists(template);
            Guard.TemplateBelongsToRequester(template.CreatedBy, owner);
            Guard.TemplateNotificationTypeIsSameAsTarget(notificationType, template.NotificationType);
            Guard.TemplateContentIsValid(template?.Content);
        }

        public async Task UpdateTemplateContent(string templateId, UpdateTemplateContentRequestDto request, string owner)
        {
            var template = await _repository.FindOneAsync(x => x.TemplateId == templateId);

            Guard.TemplateExists(template);
            Guard.TemplateBelongsToRequester(template.CreatedBy, owner);
            
            var content = request.Base64Content.DecodeBase64();
            
            Guard.TemplateHasContent(content);
            template.Content = content;
            
            await _repository.UpdateOneByIdAsync(template.Id, template);
        }
    }
}