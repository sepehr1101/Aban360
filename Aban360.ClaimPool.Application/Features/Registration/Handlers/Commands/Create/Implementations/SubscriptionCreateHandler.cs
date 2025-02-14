using Aban360.ClaimPool.Application.Features.Registration.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Registration.Dto.Command;
using Aban360.ClaimPool.Domain.Features.Registration.Entities;
using Aban360.ClaimPool.Persistence.Features.Registration.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Registration.Handlers.Commands.Create.Implementations
{
    public class SubscriptionCreateHandler : ISubscriptionCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ISubscriptionCommandService _commandService;
        public SubscriptionCreateHandler(
            IMapper mapper,
            ISubscriptionCommandService commandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));
        }

        public async Task Handle(SubscriptionCreateDto createDto, CancellationToken cancellationToken)
        {
            var subscription = _mapper.Map<Subscription>(createDto);
            await _commandService.Add(subscription);
        }
    }
}
