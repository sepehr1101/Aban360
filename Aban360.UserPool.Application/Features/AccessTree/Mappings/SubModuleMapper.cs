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
            CreateMap<SubModuleCreateDto, SubModule>();
            CreateMap<SubModuleDeleteDto, SubModule>();
            CreateMap<SubModuleUpdateDto, SubModule>();
            CreateMap<SubModule,SubModuleGetDto>()
                .ForMember(dest => dest.ModuleTitle, opt => opt.MapFrom(src => src.Module.Title));

        }
    }
}
