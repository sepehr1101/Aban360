using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Mappings
{
    public class RequestWaterMeterSiphonMapper : Profile
    {
        public RequestWaterMeterSiphonMapper()
        {
            CreateMap<WaterMeterSiphonRequestCreateDto, RequestWaterMeterSiphon>();
            CreateMap<WaterMeterSiphonRequestDeleteDto, RequestWaterMeterSiphon>();
            CreateMap<WaterMeterSiphonRequestUpdateDto, RequestWaterMeterSiphon>();
        }
    }
}
