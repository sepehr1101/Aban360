using Aban360.ClaimPool.Domain.Features.Registration.Dto.Command;
using Aban360.ClaimPool.Domain.Features.Registration.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Registration.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Registration.Mappings
{
    public class UseStateMapper:Profile
    {
        public UseStateMapper()
        {
            CreateMap<UseStateCreateDto, UseState >().ReverseMap();
            CreateMap<UseStateDeleteDto, UseState >().ReverseMap();
            CreateMap<UseStateUpdateDto, UseState >().ReverseMap();
            CreateMap<UseStateGetDto, UseState>().ReverseMap();
        }
    }
}
