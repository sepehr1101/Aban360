using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Mappers
{
    public class SubscriptionTypeMapper : Profile
    {
        public SubscriptionTypeMapper()
        {
            CreateMap<SubscriptionTypeCreateDto, SubscriptionType>();
            CreateMap<SubscriptionTypeDeleteDto, SubscriptionType>();
            CreateMap<SubscriptionTypeUpdateDto, SubscriptionType>();
            CreateMap<SubscriptionType,SubscriptionTypeGetDto>();
        }
    }
}
