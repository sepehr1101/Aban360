using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Queries;
using Aban360.MeterPool.Domain.Features.Management.Entities;
using AutoMapper;

namespace Aban360.MeterPool.Application.Features.Management.Mappings
{
    public class ReadingConfigDefaultMapper:Profile
    {
        public ReadingConfigDefaultMapper()
        {
            CreateMap<ReadingConfigDefaultCreateDto,ReadingConfigDefault>();
            CreateMap<ReadingConfigDefaultDeleteDto,ReadingConfigDefault>();
            CreateMap<ReadingConfigDefaultUpdateDto,ReadingConfigDefault>();
            CreateMap<ReadingConfigDefault,ReadingConfigDefaultGetDto>();
        }
    }
}
