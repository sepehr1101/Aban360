using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using DNTPersianUtils.Core;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    internal sealed class SubscriptionAssignmentUpdateHandler : ISubscriptionAssignmentUpdateHandler
    {
        private readonly ISubscriptionAssignmentQueryService _subscriptionAssignmentQueryService;
        private readonly ISubscriptionAssignmentCommandService _subscriptionAssignmentCommandService;
        public SubscriptionAssignmentUpdateHandler(
            ISubscriptionAssignmentQueryService subscriptionAssignmentQueryService,
            ISubscriptionAssignmentCommandService subscriptionAssignmentCommandService)
        {
            _subscriptionAssignmentQueryService = subscriptionAssignmentQueryService;
            _subscriptionAssignmentQueryService.NotNull(nameof(subscriptionAssignmentQueryService));

            _subscriptionAssignmentCommandService = subscriptionAssignmentCommandService;
            _subscriptionAssignmentCommandService.NotNull(nameof(subscriptionAssignmentCommandService));
        }

        public async Task Handle(SubscriptionAssignmentUpdateDto updateDto, CancellationToken cancellationToken)
        {
            SubscriptionAssignmentGetDto previousSubscription = await _subscriptionAssignmentQueryService.Get(updateDto.BillId);
            if (previousSubscription == null)
            {
                throw new BaseException("شناسه قبض یافت نشد");
            }

            //update
            await _subscriptionAssignmentCommandService.Update(updateDto, DateTime.Now.ToShortPersianDateString());
        }
    }
}
