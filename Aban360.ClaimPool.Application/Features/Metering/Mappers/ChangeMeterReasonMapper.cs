using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Mappers
{
    public class ChangeMeterReasonMapper : Profile
    {
        public ChangeMeterReasonMapper()
        {
            CreateMap<ChangeMeterReasonCreateDto, ChangeMeterReason>();
            CreateMap<ChangeMeterReasonDeleteDto, ChangeMeterReason>();
            CreateMap<ChangeMeterReasonUpdateDto, ChangeMeterReason>();
            CreateMap<ChangeMeterReason,ChangeMeterReasonGetDto>();
        }
    }
}
