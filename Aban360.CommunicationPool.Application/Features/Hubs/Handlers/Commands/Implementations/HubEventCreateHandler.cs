using Aban360.Common.Extensions;
using Aban360.CommunicationPool.Application.Features.Hubs.Handlers.Commands.Contracts;
using Aban360.CommunicationPool.Domain.Features.Hubs.Dto.Commands;
using Aban360.CommunicationPool.Persistence.Features.Hubs.Commands.Contracts;
using FluentValidation;
using System.Transactions;

namespace Aban360.CommunicationPool.Application.Features.Hubs.Handlers.Commands.Implementations
{
    internal sealed class HubEventCreateHandler : IHubEventCreateHandler
    {
        private readonly IHubEventCreateService _hubEventQueryService;
        private readonly IHubEventUpdateService _hubEventUpdateService;
        public HubEventCreateHandler(
            IHubEventCreateService hubEventQueryService,
            IHubEventUpdateService hubEventUpdateService)
        {
            _hubEventQueryService = hubEventQueryService;
            _hubEventQueryService.NotNull(nameof(hubEventQueryService));

            _hubEventUpdateService = hubEventUpdateService;
            _hubEventQueryService.NotNull(nameof(_hubEventUpdateService));
        }

        public async Task Handle(HubEventCreateDto input, CancellationToken cancellationToken)
        {
            //using (TransactionScope transaction=TransactionBuilder.Create(0, 5))
            //{
                //await _hubEventUpdateService.CloseAllConnection(input.UserId);
                await _hubEventQueryService.Create(input);
            //    transaction.Complete();
            //}
        }
    }
}
