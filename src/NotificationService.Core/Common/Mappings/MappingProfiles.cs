using AutoMapper;
using NotificationService.Common.Entities;
using NotificationService.Common.Dtos;
using NotificationService.Contracts.RequestDtos;
using NotificationService.Contracts.ResponseDtos;
using NotificationService.Core.Common.Utils;
namespace NotificationService.Core.Common.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Template, TemplateDTO>();
            CreateMap<Notification, NotificationDTO>();
            CreateMap<Notification, NotificationDetailDto>();
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
            CreateMap<SendGridSettingDTO, SendGridSetting>();
            CreateMap<SendGridSetting, SendGridSettingDTO>()
                .ForMember(dest => dest.ApiKey, opt => opt.MapFrom(src => SecretMasker.Mask(src.ApiKey)));
            
            CreateMap<SMTPSettingDTO, SMTPSetting>();
            CreateMap<SMTPSetting, SMTPSettingDTO>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => SecretMasker.Mask(src.Password)));

            CreateMap<HttpClientSetting, HttpClientSettingDTO>().ReverseMap();
            CreateMap<HttpClientParam, HttpClientParamDTO>().ReverseMap();
            CreateMap<ProviderDevSettings, ProviderDevSettingsDTO>().ReverseMap();
            CreateMap<Attachment, AttachmentDTO>().ReverseMap();
            CreateMap<AttachmentDTO,AttachmentContentDto>().ReverseMap();
            CreateMap<JsonBodyDTO, JsonBody>().ReverseMap();
            CreateMap<JsonKeyDTO, JsonKey>().ReverseMap();

            CreateMap<JsonBody, Providers.Libraries.JSONParser.JsonBody>();
            CreateMap<JsonKey, Providers.Libraries.JSONParser.JsonKey>();
            CreateMap<MetadataDto, Providers.Libraries.JSONParser.Metadata>();
        }
    }
}