using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NotificationService.Repositories;
using NotificationService.Dtos;
using NotificationService.Entities;
using NotificationService.Utils;
using NotificationService.Enums;
using NotificationService.Dtos.Requests;
using NotificationService.Services.Interfaces;

namespace NotificationService.Controllers
{   
    [ApiController]
    [ApiVersion(ApiVersions.v1)]
    [Route(Routes.ControllerRoute)]
    public class TemplatesController : ApiController
    {
        private readonly IRepository<Template> _repository;
        private readonly ITemplateService _templateService;

        public TemplatesController(IRepository<Template> repository, ITemplateService templateService)
        {
            _repository = repository;
            _templateService = templateService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string name, string subject, string platformName, int? page, int? pageSize)
        {
            var response = await _templateService.GetTemplates(x => (x.Name == name || name == null) && (x.Subject == subject || subject == null) && (x.PlatformName == platformName  || platformName == null), owner: Owner, page, pageSize);
            return Ok(response);
        }

        [HttpGet("{templateId}")]
        public async Task<IActionResult> GetById([FromRoute] string templateId)
        {
            var response = await _templateService.GetTemplateById(templateId, Owner);
            if (response?.Data == null) return NotFound();
            return Ok(response);
        }

        [HttpGet("{templateId}/preview")]
        public async Task<ContentResult> GetHtml(string templateId)
        {
            var template = await _repository.FindOneAsync(x => x.TemplateId == templateId && x.CreatedBy == Owner);
            
            if (template == null)
            {
                return new ContentResult { StatusCode = 404, Content = "Not Found" };
            }

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = 200,
                Content = template?.Content
            };
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateTemplateRequestDto request)
        {
            var templateCreated = await _templateService.CreateTemplate(request, owner: Owner);
            return StatusCode(StatusCodes.Status201Created, templateCreated);
        }

        [HttpDelete("{templateId}")]
        public async Task<IActionResult> Delete([FromRoute] string templateId)
        {
            await _templateService.DeleteTemplate(templateId, Owner);
            return Ok();
        }       
    }
}