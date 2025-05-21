using Aban360.ClaimPool.Domain.Features._Base.Dto;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using AutoMapper;
using WaterMeterCommandBaseDto = Aban360.ClaimPool.Domain.Features._Base.Dto.WaterMeterCommandBaseDto;

namespace Aban360.ClaimPool.Application.Features.Draft.Mappings
{
    internal class RequestSubscriptionMapper : Profile
    {
        public RequestSubscriptionMapper()
        {
            //Create
            CreateMap<WaterMeterCommandBaseDto, RequestWaterMeter>();
            CreateMap<SiphonCommandBaseDto, RequestSiphon>();
            CreateMap<EstateCommandBaseDto, RequestEstate>();
            CreateMap<FlatCommandBaseDto, RequestFlat>();
            CreateMap<IndividualCommandBaseDto, RequestIndividual>();
            CreateMap<EstateCommandDto, RequestEstate>();
            CreateMap<FlatCommandDto, RequestFlat>();
            CreateMap<IndividualCommandDto, RequestIndividual>();
            CreateMap<SiphonCommandDto, RequestSiphon>();
            CreateMap<WaterMeterCommandDto, RequestWaterMeter>();
            CreateMap<WaterMeterSiphonCommandDto, RequestWaterMeterSiphon>();
            CreateMap<WaterMeterTagCommandDto, RequestWaterMeterTag>();
            CreateMap<IndividualEstateCommandDto, RequestIndividualEstate>();
            CreateMap<IndividualTagCommandDto, RequestIndividualTag>();
            CreateMap<RequestUser, RequestUserQueryDto>();

            //Get
            CreateMap<RequestWaterMeter, RequestWaterMeterGetDto>();
            CreateMap<RequestIndividual, RequestIndividualGetDto>();
            CreateMap<RequestIndividualTag, RequestIndividualTagGetDto>();
            CreateMap<RequestIndividualDiscountType, RequestIndividualDiscountTypeGetDto>();
            CreateMap<RequestEstate, RequestEstateGetDto>();
            CreateMap<RequestFlat, RequestFlatGetDto>();
            CreateMap<RequestSiphon, RequestSiphonGetDto>();
        }
    }
}
