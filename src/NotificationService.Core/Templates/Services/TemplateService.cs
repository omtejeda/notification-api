using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using NotificationService.Common.Entities;
using NotificationService.Common.Enums;
using NotificationService.Core.Common.Exceptions;
using LinqKit;
using NotificationService.Core.Common.Utils;
using NotificationService.Common.Dtos;
using NotificationService.Common.Models;
using NotificationService.Contracts.RequestDtos;
using NotificationService.Contracts.Interfaces.Services;
using NotificationService.Contracts.Interfaces.Repositories;

namespace NotificationService.Core.Templates.Services
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
        public async Task<FinalResponseDto<TemplateDto>> CreateTemplate(CreateTemplateRequestDto request, string owner)
        {
            Enum.TryParse(request.NotificationType, out NotificationType notificationType);

            if (notificationType == NotificationType.None)
                throw new RuleValidationException($"Notification type [{request.NotificationType}] not valid");

            var existingTemplate = await FindByCompositeKeyAsync(request.Name, platformName: owner, request.Language, owner);
            
            if (existingTemplate is not null)
                throw new RuleValidationException("Template already exists");

            var metadata = request.Metadata.Select(x => new Metadata { Key = x.Key, Description = x.Description, IsRequired = x.IsRequired }).ToList();
            var labels = _mapper.Map<ICollection<TemplateLabel>>(request.Labels);

            foreach(var label in labels.Where(x => !string.IsNullOrWhiteSpace(x.CatalogNameToCheckAgainst)))
            {
                var catalog = await _catalogRepository.FindOneAsync(x => x.Name == label.CatalogNameToCheckAgainst);
                if (catalog is null) throw new RuleValidationException($"Catalog [{label.CatalogNameToCheckAgainst}] does not exist");
                if (!catalog.Elements.Any(x => x.Key == label.Value)) throw new RuleValidationException($"Catalog [{catalog.Name}] does not have key: [{label.Value}]");
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
            var templateDTO = _mapper.Map<TemplateDto>(entity);
            return new FinalResponseDto<TemplateDto>((int) ErrorCode.OK, templateDTO);
        }
        public async Task DeleteTemplate(string templateId, string owner)
        {
            var existingTemplate = await _repository.FindOneAsync(x => x.TemplateId == templateId);

            if (existingTemplate.CreatedBy != owner)
                throw new RuleValidationException($"Template was not created by {owner}");

            if (existingTemplate is null)
                throw new RuleValidationException("Template you're trying to delete does not exist");
            
            await _repository.DeleteOneAsync(x => x.Id == existingTemplate.Id);
        }

        private async Task<Template> FindByCompositeKeyAsync(string name, string platformName, Language language, string owner)
        {
            var (templates, _) = await _repository.FindAsync(x => x.Name == name && x.PlatformName == platformName && x.CreatedBy == owner);
            var template = templates.FirstOrDefault(x => x.Language == language);

            return template;
        }

        public async Task<FinalResponseDto<IEnumerable<TemplateDto>>> GetTemplates(Expression<Func<Template, bool>> filter, string owner, int? page, int? pageSize)
        {
            var filterByOwner = PredicateBuilder.New<Template>().And(x => x.CreatedBy == owner).Expand();
            filter = filter.And(filterByOwner);
            
            var (templates, pagination) = await _repository.FindAsync(filter, page, pageSize);
            var templatesDTO = _mapper.Map<IEnumerable<TemplateDto>>(templates);
            var paginationDTO = _mapper.Map<PaginationDto>(pagination);

            return new FinalResponseDto<IEnumerable<TemplateDto>>( (int) ErrorCode.OK, templatesDTO, paginationDTO);
        }

        public async Task<FinalResponseDto<TemplateDto>> GetTemplateById(string templateId, string owner)
        {
            var template = await _repository.FindOneAsync(x => x.TemplateId == templateId);

            if (template is null) return default;
            
            if (template.CreatedBy != owner)
                throw new RuleValidationException($"Template was not created by platform {owner}");
            
            var templateDTO = _mapper.Map<TemplateDto>(template);

            return new FinalResponseDto<TemplateDto>((int) ErrorCode.OK, templateDTO);
        }

        public async Task<RuntimeTemplate> GetRuntimeTemplate(string name, string platformName, Language language, List<MetadataDto> providedMetadata, string owner, NotificationType notificationType)
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
            if (template is null)
                throw new RuleValidationException("Couldn't find template");
            
            if (template.CreatedBy != owner && template.PlatformName != owner)
                throw new RuleValidationException("Template does not belong to your platform!");

            if (template.NotificationType != notificationType)
                throw new RuleValidationException($"Template specified does not correspond to {notificationType}. It corresponds to {template.NotificationType}");

            if (template?.Content is null)
                throw new RuleValidationException("Template provided does not have content");
        }

        public async Task UpdateTemplateContent(string templateId, UpdateTemplateContentRequestDto request, string owner)
        {
            var template = await _repository.FindOneAsync(x => x.TemplateId == templateId);
            if (template is null)
                throw new RuleValidationException("Template does not exist");

            if (template.CreatedBy != owner)
                throw new RuleValidationException($"Template was not created by platform {owner}");

            
            var content = request.Base64Content.DecodeBase64();

            if (string.IsNullOrWhiteSpace(content))
                throw new RuleValidationException("Content provided not valid");

            template.Content = content;
            
            await _repository.UpdateOneByIdAsync(template.Id, template);
        }
    }
}