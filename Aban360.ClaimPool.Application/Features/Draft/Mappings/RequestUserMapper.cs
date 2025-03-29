using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Mappings
{
    internal class RequestUserMapper : Profile
    {
        public RequestUserMapper()
        {
            CreateMap<RequestUserCommandDto, RequestUser>()
            .ForMember(dest => dest.RequestEstate, opt => opt.MapFrom(src => src.EstateCommand))
            .ForMember(dest => dest.RequestFlat, opt => opt.MapFrom(src => src.flatCommands))
            .ForMember(dest => dest.RequestIndividual, opt => opt.MapFrom(src => src.IndividualCommand))
            .ForMember(dest => dest.RequestSiphon, opt => opt.MapFrom(src => src.SiphonCommand))
            .ForMember(dest => dest.RequestWaterMeter, opt => opt.MapFrom(src => src.WaterMeterCommand))
            .ForMember(dest => dest.RequestWaterMeterSiphon, opt => opt.MapFrom(src => src.WaterMeterSiphonCommand))
            .ForMember(dest => dest.RequestWaterMeterTag, opt => opt.MapFrom(src => src.WaterMeterTagCommand))
            .ForMember(dest => dest.RequestIndividualEstate, opt => opt.MapFrom(src => src.IndividualEstateCommand))
            .ForMember(dest => dest.RequestIndividualTag, opt => opt.MapFrom(src => src.IndividualTagCommand));

            CreateMap<RequestUser, RequestUserQueryDto>();

        }
    }
}
