using Aban360.ClaimPool.Application.Features.Registration.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Registration.Dto.Command;
using Aban360.ClaimPool.Persistence.Features.Registration.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Registration.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Registration.Handlers.Commands.Delete.Implementations
{
    public class SubscriptionDeleteHandler : ISubscriptionDeleteHandler
    {
        private readonly ISubscriptionCommandService _commandService;
        private readonly ISubscriptionQueryService _queryService;
        public SubscriptionDeleteHandler(
            ISubscriptionCommandService commandService,
            ISubscriptionQueryService queryService)
        {
            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));

            _queryService = queryService;
            _commandService.NotNull(nameof(queryService));
        }

        public async Task Handle(SubscriptionDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var subscription = await _queryService.Get(deleteDto.Id);
            if (subscription == null)
            {
                throw new InvalidDataException();
            }
            await _commandService.Remove(subscription);
        }
    }
}
