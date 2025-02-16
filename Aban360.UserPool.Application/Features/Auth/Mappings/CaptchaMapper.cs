using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.Auth.Mappings
{
    public class CaptchaMapper : Profile
    {
        public CaptchaMapper()
        {
            CreateMap<Captcha, CaptchaActiveDto>()
                .ForMember(dest => dest.DisplayModeEnumId, opt => opt.MapFrom(src => src.CaptchaDisplayMode.DisplayModeEnumId))
                .ForMember(dest => dest.LanguageId, opt => opt.MapFrom(src => src.CaptchaLanguage.LanguageId));


            CreateMap<Captcha, CaptchaSingleDto>()
               .ForMember(dest => dest.DisplayModeEnumId, opt => opt.MapFrom(src => src.CaptchaDisplayMode.Id))
               .ForMember(dest => dest.DisplayModeTitle, opt => opt.MapFrom(src => src.CaptchaDisplayMode.Title))
               .ForMember(dest => dest.LanguageId, opt => opt.MapFrom(src => src.CaptchaLanguage.Id))
               .ForMember(dest => dest.LanguageTitle, opt => opt.MapFrom(src => src.CaptchaLanguage.Title));

            CreateMap<Captcha, CaptchaListQueryDto>();
            //.ForMember(dest => dest.CaptchaDisplayModeId, opt => opt.MapFrom(src => src.CaptchaDisplayModeId))
            //.ForMember(dest => dest.CaptchaLanguageId, opt => opt.MapFrom(src => src.CaptchaLanguageId));

            CreateMap<CaptchaUpdateDto, Captcha>().ReverseMap();
            CreateMap<CaptchaCreateDto, Captcha>().ReverseMap();
        }
    }
}
