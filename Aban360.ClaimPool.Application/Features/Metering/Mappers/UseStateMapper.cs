using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Mappers
{
    public class UseStateMapper : Profile
    {
        public UseStateMapper()
        {
            CreateMap<UseStateCreateDto, UseState>().ReverseMap();
            CreateMap<UseStateDeleteDto, UseState>().ReverseMap();
            CreateMap<UseStateUpdateDto, UseState>().ReverseMap();
            CreateMap<UseStateGetDto, UseState>().ReverseMap();
        }
    }
}
