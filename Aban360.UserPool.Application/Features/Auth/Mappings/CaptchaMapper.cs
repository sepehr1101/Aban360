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

            CreateMap<Captcha, CaptchaQueryDto>()
               .ForMember(dest => dest.DisplayModeId, opt => opt.MapFrom(src => src.CaptchaDisplayMode.Id))
               .ForMember(dest => dest.DispalyModeTitle, opt => opt.MapFrom(src => src.CaptchaDisplayMode.Title))
               .ForMember(dest => dest.LanguageId, opt => opt.MapFrom(src => src.CaptchaLanguage.Id))
               .ForMember(dest => dest.LanguageTitle, opt => opt.MapFrom(src => src.CaptchaLanguage.Title));

            CreateMap<CaptchaUpdateDto, Captcha>().ReverseMap();
            CreateMap<CaptchaCreateDto, Captcha>().ReverseMap();
        }
    }
}
