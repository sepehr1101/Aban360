using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries;
using Aban360.UserPool.Domain.Features.AceessTree.Entites;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.AccessTree.Mappings
{
    public class EndpointMapper : Profile
    {
        public EndpointMapper()
        {
            CreateMap<EndpointCreateDto, Endpoint>()
                .ReverseMap();

            CreateMap<EndpointDeleteDto, Endpoint>()
                .ReverseMap();

            CreateMap<EndpointUpdateDto, Endpoint>()
                .ReverseMap();

            CreateMap<EndpointGetDto, Endpoint>()
                .ReverseMap()
                .ForMember(dest => dest.SubModuleTitle, opt => opt.MapFrom(src => src.SubModule.Title));

        }
    }
}
