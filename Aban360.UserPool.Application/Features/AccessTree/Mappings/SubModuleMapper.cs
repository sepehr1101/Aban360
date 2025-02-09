using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries;
using Aban360.UserPool.Domain.Features.AceessTree.Entites;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.AccessTree.Mappings
{
    public class SubModuleMapper : Profile
    {
        public SubModuleMapper()
        {
            CreateMap<SubModuleCreateDto, SubModule>()
                .ReverseMap();

            CreateMap<SubModuleDeleteDto, SubModule>()
                .ReverseMap();

            CreateMap<SubModuleUpdateDto, SubModule>()
                .ReverseMap();

            CreateMap<SubModuleGetDto, SubModule>()
                .ReverseMap()
                .ForMember(dest => dest.ModuleTitle, opt => opt.MapFrom(src => src.Module.Title));

        }
    }
}
