using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.Auth.Mappings
{
    public class CaptchaMapper :Profile
    {
        public CaptchaMapper()
        {
            CreateMap<Captcha,CaptchaSingleQueryDto>()
                .ForMember(dest=> dest.DisplayModeEnumId, opt=>opt.MapFrom(src=>src.CaptchaDisplayMode.DisplayModeEnumId))
                .ForMember(dest => dest.LanguageId, opt => opt.MapFrom(src => src.CaptchaLanguage.LanguageId));

            CreateMap<Captcha, CaptchaListQueryDto>();
                //.ForMember(dest => dest.CaptchaDisplayModeId, opt => opt.MapFrom(src => src.CaptchaDisplayModeId))
                //.ForMember(dest => dest.CaptchaLanguageId, opt => opt.MapFrom(src => src.CaptchaLanguageId));

            CreateMap<CaptchaUpdateDto, Captcha>();
        }
    }
}
