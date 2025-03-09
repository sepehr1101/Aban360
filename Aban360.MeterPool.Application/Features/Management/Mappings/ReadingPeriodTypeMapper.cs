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
            CreateMap<ReadingPeriodTypeCreateDto, ReadingPeriodType>().ReverseMap();
            CreateMap<ReadingPeriodTypeDeleteDto, ReadingPeriodType>().ReverseMap();
            CreateMap<ReadingPeriodTypeUpdateDto, ReadingPeriodType>().ReverseMap();
            CreateMap<ReadingPeriodTypeGetDto, ReadingPeriodType>().ReverseMap();
        }
    }
}
