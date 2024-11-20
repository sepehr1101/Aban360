using Aban360.UserPool.Domain.Features.Auth.Entities;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.Auth.Mappings
{
    public class CaptchaMapper :Profile
    {
        public CaptchaMapper()
        {
            CreateMap<Captcha,CaptchaQueryDto>()
                .ForMember(dest=> dest.DisplayModeEnumId, opt=>opt.MapFrom(src=>src.CaptchaDisplayMode.DisplayModeEnumId))
                .ForMember(dest => dest.LanguageId, opt => opt.MapFrom(src => src.CaptchaLanguage.LanguageId));
        }
    }
}
