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
            CreateMap<AppCreateDto, App>();
            CreateMap<AppDeleteDto, App>();
            CreateMap<AppUpdateDto, App>();
            CreateMap<App,AppGetDto>();
        }
    }
}
