using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Implementations
{
    public class SubscriptionTypeUpdateHandler : ISubscriptionTypeUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ISubscriptionTypeQueryService _subscriptionTypeQueryService;
        public SubscriptionTypeUpdateHandler(
            IMapper mapper,
            ISubscriptionTypeQueryService subscriptionTypeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _subscriptionTypeQueryService = subscriptionTypeQueryService;
            _subscriptionTypeQueryService.NotNull(nameof(_subscriptionTypeQueryService));
        }

        public async Task Handle(SubscriptionTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var SubscriptionType = await _subscriptionTypeQueryService.Get(updateDto.Id);
            _mapper.Map(SubscriptionType, updateDto);
        }
    }
}
