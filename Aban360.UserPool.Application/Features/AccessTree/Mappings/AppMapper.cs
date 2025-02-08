using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries;
using Aban360.UserPool.Domain.Features.AceessTree.Entites;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.AccessTree.Mappings
{
    public  class AppMapper:Profile
    {
        public AppMapper()
        {
            CreateMap<AppCreateDto, App>().ReverseMap();
            CreateMap<AppDeleteDto, App>().ReverseMap();
            CreateMap<AppUpdateDto, App>().ReverseMap();
            CreateMap<AppGetDto, App>().ReverseMap();
        }
    }
}
