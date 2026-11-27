using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Queries;
using Aban360.MeterPool.Domain.Features.Management.Entities;
using AutoMapper;

namespace Aban360.MeterPool.Application.Features.Management.Mappings
{
    public class ReadingPeriodTypeMapper : Profile
    {
        public ReadingPeriodTypeMapper()
        {
            CreateMap<ReadingPeriodTypeCreateDto, ReadingPeriodType>();
            CreateMap<ReadingPeriodTypeDeleteDto, ReadingPeriodType>();
            CreateMap<ReadingPeriodTypeUpdateDto, ReadingPeriodType>();
            CreateMap<ReadingPeriodType, ReadingPeriodTypeGetDto>();
        }
    }
}
