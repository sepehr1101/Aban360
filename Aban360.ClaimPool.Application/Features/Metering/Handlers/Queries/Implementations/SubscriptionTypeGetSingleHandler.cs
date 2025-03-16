using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Implementations
{
    internal sealed class SubscriptionTypeGetSingleHandler : ISubscriptionTypeGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly ISubscriptionTypeQueryService _subscriptionTypeQueryService;
        public SubscriptionTypeGetSingleHandler(
            IMapper mapper,
            ISubscriptionTypeQueryService subscriptionTypeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _subscriptionTypeQueryService = subscriptionTypeQueryService;
            _subscriptionTypeQueryService.NotNull(nameof(_subscriptionTypeQueryService));
        }

        public async Task<SubscriptionTypeGetDto> Handle(SubscriptionTypeEnum id, CancellationToken cancellationToken)
        {
            SubscriptionType subscriptionType = await _subscriptionTypeQueryService.Get(id);
            if (subscriptionType == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<SubscriptionTypeGetDto>(subscriptionType);
        }
    }
}
