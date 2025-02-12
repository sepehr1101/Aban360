using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Mappers
{
    public class MeterTypeMapper : Profile
    {
        public MeterTypeMapper()
        {
            CreateMap<MeterTypeCreateDto, MeterType>().ReverseMap();
            CreateMap<MeterTypeDeleteDto, MeterType>().ReverseMap();
            CreateMap<MeterTypeUpdateDto, MeterType>().ReverseMap();
            CreateMap<MeterTypeGetDto, MeterType>().ReverseMap();
        }
    }
}
