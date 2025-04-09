using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Mappings
{
    public class RequestWaterMeterMapper : Profile
    {
        public RequestWaterMeterMapper()
        {
            CreateMap<WaterMeterRequestCreateDto, RequestWaterMeter>();
            CreateMap<WaterMeterRequestDeleteDto, RequestWaterMeter>();
            CreateMap<WaterMeterRequestUpdateDto, RequestWaterMeter>();
        }
    }
}
