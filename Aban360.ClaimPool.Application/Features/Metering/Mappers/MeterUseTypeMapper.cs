using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Mappers
{
    public class MeterUseTypeMapper : Profile
    {
        public MeterUseTypeMapper()
        {
            CreateMap<MeterUseTypeCreateDto, MeterUseType>().ReverseMap();
            CreateMap<MeterUseTypeDeleteDto, MeterUseType>().ReverseMap();
            CreateMap<MeterUseTypeUpdateDto, MeterUseType>().ReverseMap();
            CreateMap<MeterUseTypeGetDto, MeterType>().ReverseMap();
        }
    }
}
