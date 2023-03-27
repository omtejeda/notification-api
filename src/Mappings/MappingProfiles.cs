using AutoMapper;
using NotificationService.Entities;
using NotificationService.Dtos;
using NotificationService.Dtos.Requests;
namespace NotificationService.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Template, TemplateDTO>();
            CreateMap<Notification, NotificationDTO>();
            CreateMap<Platform, PlatformDTO>();
            CreateMap<Provider, ProviderDTO>().ReverseMap();
            CreateMap<Pagination, PaginationDTO>();
            CreateMap<ProviderSettings, ProviderSettingsDTO>().ReverseMap();
            CreateMap<Catalog, CatalogDTO>().ReverseMap();
            CreateMap<Element, ElementDTO>().ReverseMap();
            CreateMap<Label, LabelDTO>().ReverseMap();
            CreateMap<TemplateLabel, TemplateLabelDTO>().ReverseMap();
            CreateMap<CreateProviderRequestDto, Provider>();
            CreateMap<HttpClientSetting, HttpClientSettingDTO>().ReverseMap();
            CreateMap<SendGridSetting, SendGridSettingDTO>().ReverseMap();
            CreateMap<SMTPSetting, SMTPSettingDTO>().ReverseMap();
            CreateMap<HttpClientSetting, HttpClientSettingDTO>().ReverseMap();
            CreateMap<HttpClientParam, HttpClientParamDTO>().ReverseMap();
            CreateMap<ProviderDevSettings, ProviderDevSettingsDTO>().ReverseMap();
            CreateMap<Attachment, AttachmentDTO>().ReverseMap();
        }
    }
}