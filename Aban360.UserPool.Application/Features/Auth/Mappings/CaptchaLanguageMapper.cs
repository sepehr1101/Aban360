using Aban360.UserPool.Domain.Features.Auth.Entities;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.Auth.Mappings
{
    public class CaptchaLanguageMapper:Profile
    {
        public CaptchaLanguageMapper()
        {
            CreateMap<CaptchaLanguage,CaptchaLanguageDto>().ReverseMap();
        }
    }
}
