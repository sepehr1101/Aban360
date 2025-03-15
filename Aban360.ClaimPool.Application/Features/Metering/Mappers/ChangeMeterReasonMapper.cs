using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Mappers
{
    public class ChangeMeterReasonMapper : Profile
    {
        public ChangeMeterReasonMapper()
        {
            CreateMap<ChangeMeterReasonCreateDto, ChangeMeterReason>().ReverseMap();
            CreateMap<ChangeMeterReasonDeleteDto, ChangeMeterReason>().ReverseMap();
            CreateMap<ChangeMeterReasonUpdateDto, ChangeMeterReason>().ReverseMap();
            CreateMap<ChangeMeterReasonGetDto, ChangeMeterReason>().ReverseMap();
        }
    }
}
