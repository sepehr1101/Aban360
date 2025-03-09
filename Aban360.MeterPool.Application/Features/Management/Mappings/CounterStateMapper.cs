using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Queries;
using Aban360.MeterPool.Domain.Features.Management.Entities;
using AutoMapper;

namespace Aban360.MeterPool.Application.Features.Management.Mappings
{
    public class CounterStateMapper:Profile
    {
        public CounterStateMapper()
        {
            CreateMap<CounterStateCreateDto, CounterState>().ReverseMap();
            CreateMap<CounterStateDeleteDto, CounterState>().ReverseMap();
            CreateMap<CounterStateUpdateDto, CounterState>().ReverseMap();
            CreateMap<CounterStateGetDto, CounterState>().ReverseMap();
        }
    }
}
