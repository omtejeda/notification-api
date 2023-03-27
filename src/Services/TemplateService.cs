using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using NotificationService.Entities;
using NotificationService.Enums;
using NotificationService.Repositories;
using NotificationService.Dtos;
using NotificationService.Dtos.Requests;
using NotificationService.Exceptions;
using NotificationService.Services.Interfaces;
using LinqKit;

namespace NotificationService.Services
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
        public async Task<FinalResponseDTO<TemplateDTO>> CreateTemplate(CreateTemplateRequestDto request, string owner)
        {
            Enum.TryParse(request.NotificationType, out NotificationType notificationType);

            if (notificationType == NotificationType.None)
                throw new RuleValidationException($"Notification type [{request.NotificationType}] not valid");

            var existingTemplate = await FindByCompositeKeyAsync(request.Name, platformName: owner, request.Language, owner);
            if (existingTemplate != null)
                throw new RuleValidationException("Template already exists");

            var metadata = request.Metadata.Select(x => new Metadata { Key = x.Key, Description = x.Description, IsRequired = x.IsRequired }).ToList();
            var labels = _mapper.Map<ICollection<TemplateLabel>>(request.Labels);

            foreach(var label in labels.Where(x => !string.IsNullOrWhiteSpace(x.CatalogNameToCheckAgainst)))
            {
                var catalog = await _catalogRepository.FindOneAsync(x => x.Name == label.CatalogNameToCheckAgainst);
                if (catalog == null) throw new RuleValidationException($"Catalog [{label.CatalogNameToCheckAgainst}] does not exist");
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
            var templateDTO = _mapper.Map<TemplateDTO>(entity);
            return new FinalResponseDTO<TemplateDTO>((int) ErrorCode.OK, templateDTO);
        }
        public async Task DeleteTemplate(string templateId, string owner)
        {
            var existingTemplate = await _repository.FindOneAsync(x => x.TemplateId == templateId);

            if (existingTemplate.CreatedBy != owner)
                throw new RuleValidationException($"Template was not created by {owner}");

            if (existingTemplate == null)
                throw new RuleValidationException("Template you're trying to delete does not exist");
            
            await _repository.DeleteOneAsync(x => x.Id == existingTemplate.Id);
        }

        private async Task<Template> FindByCompositeKeyAsync(string name, string platformName, Language language, string owner)
        {
            var (templates, _) = await _repository.FindAsync(x => x.Name == name && x.PlatformName == platformName && x.CreatedBy == owner);
            var template = templates.FirstOrDefault(x => x.Language == language);

            return template;
        }

        public async Task<FinalResponseDTO<IEnumerable<TemplateDTO>>> GetTemplates(Expression<Func<Template, bool>> filter, string owner, int? page, int? pageSize)
        {
            var filterByOwner = PredicateBuilder.New<Template>().And(x => x.CreatedBy == owner).Expand();
            filter = filter.And(filterByOwner);
            
            var (templates, pagination) = await _repository.FindAsync(filter, page, pageSize);
            var templatesDTO = _mapper.Map<IEnumerable<TemplateDTO>>(templates);
            var paginationDTO = _mapper.Map<PaginationDTO>(pagination);

            return new FinalResponseDTO<IEnumerable<TemplateDTO>>( (int) ErrorCode.OK, templatesDTO, paginationDTO);
        }

        public async Task<FinalResponseDTO<TemplateDTO>> GetTemplateById(string templateId, string owner)
        {
            var template = await _repository.FindOneAsync(x => x.TemplateId == templateId);

            if (template == null) return default;
            
            if (template.CreatedBy != owner)
                throw new RuleValidationException($"Template was not created by platform {owner}");
            
            var templateDTO = _mapper.Map<TemplateDTO>(template);

            return new FinalResponseDTO<TemplateDTO>((int) ErrorCode.OK, templateDTO);
        }
    }
}