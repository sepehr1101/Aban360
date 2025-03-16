using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Queries;
using Aban360.MeterPool.Domain.Features.Management.Entities;
using AutoMapper;

namespace Aban360.MeterPool.Application.Features.Management.Mappings
{
    public class ReadingPeriodMapper:Profile
    {
        public ReadingPeriodMapper()
        {
            CreateMap<ReadingPeriodCreateDto,ReadingPeriod>();
            CreateMap<ReadingPeriodDeleteDto,ReadingPeriod>();
            CreateMap<ReadingPeriodUpdateDto,ReadingPeriod>();
            CreateMap<ReadingPeriod, ReadingPeriodGetDto>()
                .ForMember(dest => dest.ReadingPeriodTypeTitle, opt => opt.MapFrom(src => src.ReadingPeriodType.Title));
        }
    }
}
