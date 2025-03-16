using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries;
using Aban360.UserPool.Domain.Features.AceessTree.Entites;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.AccessTree.Mappings
{
    public class ModuleMapper : Profile
    {
        public ModuleMapper()
        {
            CreateMap<ModuleCreateDto, Module>();
            CreateMap<ModuleDeleteDto, Module>();
            CreateMap<ModuleUpdateDto, Module>();
            CreateMap<Module,ModuleGetDto>()
                .ForMember(dest => dest.AppTitle, opt => opt.MapFrom(src => src.App.Title));
        
        }
    }
}
