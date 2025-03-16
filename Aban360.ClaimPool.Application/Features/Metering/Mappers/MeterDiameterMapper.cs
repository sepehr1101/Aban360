using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Mappers
{
    public class MeterDiameterMapper : Profile
    {
        public MeterDiameterMapper()
        {
            CreateMap<MeterDiameterCreateDto, MeterDiameter>();
            CreateMap<MeterDiameterDeleteDto, MeterDiameter>();
            CreateMap<MeterDiameterUpdateDto, MeterDiameter>();
            CreateMap<MeterDiameter,MeterDiameterGetDto>();
        }
    }
}
