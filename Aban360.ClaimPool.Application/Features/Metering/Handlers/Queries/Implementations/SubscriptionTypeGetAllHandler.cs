using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Implementations
{
    public class SubscriptionTypeGetAllHandler : ISubscriptionTypeGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly ISubscriptionTypeQueryService _subscriptionTypeQueryService;
        public SubscriptionTypeGetAllHandler(
            IMapper mapper,
            ISubscriptionTypeQueryService subscriptionTypeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _subscriptionTypeQueryService = subscriptionTypeQueryService;
            _subscriptionTypeQueryService.NotNull(nameof(_subscriptionTypeQueryService));
        }

        public async Task<ICollection<SubscriptionTypeGetDto>> Handle(CancellationToken cancellationToken)
        {
            var subscriptionType = await _subscriptionTypeQueryService.Get();
            if (subscriptionType == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<ICollection<SubscriptionTypeGetDto>>(subscriptionType);
        }
    }
}
