using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Mappings
{
    public class RequestWaterMeterTagMapper : Profile
    {
        public RequestWaterMeterTagMapper()
        {
            CreateMap<WaterMeterTagRequestCreateDto, RequestWaterMeterTag>();
            CreateMap<WaterMeterTagRequestDeleteDto, RequestWaterMeterTag>();
            CreateMap<WaterMeterTagRequestUpdateDto, RequestWaterMeterTag>();
        }
    }
}
