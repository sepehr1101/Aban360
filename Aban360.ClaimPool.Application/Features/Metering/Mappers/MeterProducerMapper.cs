using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Mappers
{
    public class MeterProducerMapper : Profile
    {
        public MeterProducerMapper()
        {
            CreateMap<MeterProducerCreateDto, MeterProducer>();
            CreateMap<MeterProducerDeleteDto, MeterProducer>();
            CreateMap<MeterProducerUpdateDto, MeterProducer>();
            CreateMap<MeterProducer,MeterProducerGetDto>();
        }
    }
}
