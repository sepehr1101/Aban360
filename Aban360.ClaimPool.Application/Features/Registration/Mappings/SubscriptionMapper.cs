using Aban360.ClaimPool.Domain.Features.Registration.Dto.Command;
using Aban360.ClaimPool.Domain.Features.Registration.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Registration.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Registration.Mappings
{
    public class SubscriptionMapper:Profile
    {
        public SubscriptionMapper()
        {
            CreateMap<SubscriptionCreateDto, Subscription>().ReverseMap();
            CreateMap<SubscriptionDeleteDto, Subscription>().ReverseMap();
            CreateMap<SubscriptionUpdateDto, Subscription>().ReverseMap();
            CreateMap<SubscriptionGetDto, Subscription>().ReverseMap();
        }
    }
}
