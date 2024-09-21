using AutoMapper;
using NotificationService.Domain.Entities;
using NotificationService.Application.Contracts.RequestDtos;
using NotificationService.Application.Contracts.ResponseDtos;
using NotificationService.Application.Utils;
using NotificationService.Domain.Models;
using NotificationService.Domain.Dtos;
using NotificationService.Application.Common.Dtos;

namespace NotificationService.Application.Common.Mappings;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Template, TemplateDto>();
        CreateMap<Notification, NotificationDto>();
        CreateMap<Notification, NotificationDetailDto>();
        CreateMap<Platform, PlatformDto>();
        CreateMap<Provider, ProviderDto>().ReverseMap();
        CreateMap<Pagination, PaginationDto>();
        CreateMap<ProviderSettings, ProviderSettingsDto>().ReverseMap();
        CreateMap<Catalog, CatalogDto>().ReverseMap();
        CreateMap<Element, ElementDto>().ReverseMap();
        CreateMap<Label, LabelDto>().ReverseMap();
        CreateMap<TemplateLabel, TemplateLabelDto>().ReverseMap();
        CreateMap<CreateProviderRequestDto, Provider>();
        CreateMap<HttpClientSetting, HttpClientSettingDto>().ReverseMap();
        CreateMap<SendGridSettingDto, SendGridSetting>();
        CreateMap<SendGridSetting, SendGridSettingDto>()
            .ForMember(dest => dest.ApiKey, opt => opt.MapFrom(src => SecretMasker.Mask(src.ApiKey)));
        
        CreateMap<SMTPSettingDto, SMTPSetting>();
        CreateMap<SMTPSetting, SMTPSettingDto>()
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => SecretMasker.Mask(src.Password)));

        CreateMap<HttpClientSetting, HttpClientSettingDto>().ReverseMap();
        CreateMap<HttpClientParam, HttpClientParamDto>().ReverseMap();
        CreateMap<ProviderDevSettings, ProviderDevSettingsDto>().ReverseMap();
        CreateMap<Attachment, AttachmentDto>().ReverseMap();
        CreateMap<AttachmentDto,AttachmentContentDto>().ReverseMap();
        CreateMap<JsonBodyDto, JsonBody>().ReverseMap();
        CreateMap<JsonKeyDto, JsonKey>().ReverseMap();

        CreateMap<JsonBody, Features.Providers.Libraries.JSONParser.JsonBody>();
        CreateMap<JsonKey, Features.Providers.Libraries.JSONParser.JsonKey>();
        CreateMap<MetadataDto, Features.Providers.Libraries.JSONParser.Metadata>();
    }
}