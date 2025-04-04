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
            //CreateMap<RequestSubscriptionCreateDto, RequestUser>()
            //.ForMember(dest => dest.RequestEstate, opt => opt.MapFrom(src => src.Estate))
            //.ForMember(dest => dest.RequestIndividual, opt => opt.MapFrom(src => src.IndividualCommand))
            //.ForMember(dest => dest.RequestSiphon, opt => opt.MapFrom(src => src.SiphonCommand))
            //.ForMember(dest => dest.RequestWaterMeter, opt => opt.MapFrom(src => src.WaterMeterCommand))
            //.ForMember(dest => dest.RequestWaterMeterSiphon, opt => opt.MapFrom(src => src.WaterMeterSiphonCommand))
            //.ForMember(dest => dest.RequestWaterMeterTag, opt => opt.MapFrom(src => src.WaterMeterTagCommand))
            //.ForMember(dest => dest.RequestIndividualEstate, opt => opt.MapFrom(src => src.IndividualEstateCommand))
            //.ForMember(dest => dest.RequestIndividualTag, opt => opt.MapFrom(src => src.IndividualTagCommand));
            CreateMap<WaterMeterCommandBaseDto, RequestWaterMeter>();
            CreateMap<SiphonCommandBaseDto, RequestSiphon>();
            CreateMap<EstateCommandBaseDto, RequestEstate>();
            CreateMap<Domain.Features._Base.Dto.FlatCommandBaseDto, RequestFlat>();
            CreateMap<IndividualCommandBaseDto, RequestIndividual>();

            CreateMap<EstateCommandDto, RequestEstate>();
            CreateMap<Domain.Features.Draft.Dto.Commands.FlatCommandDto, RequestFlat>();
            CreateMap<IndividualCommandDto, RequestIndividual>();
            CreateMap<SiphonCommandDto, RequestSiphon>();
            CreateMap<WaterMeterCommandDto, RequestWaterMeter>();
            CreateMap<WaterMeterSiphonCommandDto, RequestWaterMeterSiphon>();
            CreateMap<WaterMeterTagCommandDto, RequestWaterMeterTag>();
            CreateMap<IndividualEstateCommandDto, RequestIndividualEstate>();
            CreateMap<IndividualTagCommandDto, RequestIndividualTag>();

            CreateMap<RequestUser, RequestUserQueryDto>();

        }
    }
}
