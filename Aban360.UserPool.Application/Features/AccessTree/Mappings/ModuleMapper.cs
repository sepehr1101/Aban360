﻿using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries;
using Aban360.UserPool.Domain.Features.AceessTree.Entites;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.AccessTree.Mappings
{
    public class ModuleMapper : Profile
    {
        public ModuleMapper()
        {
            CreateMap<ModuleCreateDto, Module>()
                .ReverseMap();
                //.ForMember(dest => dest.AppTitle, opt => opt.MapFrom(src => src.App.Title));

            CreateMap<ModuleDeleteDto, Module>().ReverseMap();

            CreateMap<ModuleUpdateDto, Module>()
                .ReverseMap();
               // .ForMember(dest => dest.AppTitle, opt => opt.MapFrom(src => src.App.Title));

            CreateMap<ModuleGetDto, Module>()
                .ReverseMap()
                .ForMember(dest => dest.AppTitle, opt => opt.MapFrom(src => src.App.Title));
        
             //todo : AppTitle always need ?
        }
    }
}
