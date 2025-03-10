using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Implementations
{
    public class SubscriptionTypeCreateHandler : ISubscriptionTypeCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ISubscriptionTypeCommandService _subscriptionTypeCommandService;
        public SubscriptionTypeCreateHandler(
            IMapper mapper,
            ISubscriptionTypeCommandService subscriptionTypeCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _subscriptionTypeCommandService = subscriptionTypeCommandService;
            _subscriptionTypeCommandService.NotNull(nameof(_subscriptionTypeCommandService));
        }

        public async Task Handle(SubscriptionTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            var subscriptionType = _mapper.Map<SubscriptionType>(createDto);
            await _subscriptionTypeCommandService.Add(subscriptionType);
        }
    }
}
