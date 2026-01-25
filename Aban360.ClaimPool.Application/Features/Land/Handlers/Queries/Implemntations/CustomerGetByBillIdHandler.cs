using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    internal sealed class CustomerGetByBillIdHandler : ICustomerGetByBillIdHandler
    {
        private readonly ISubscriptionQueryService _subscriptionAssignmentQueryService;
        public CustomerGetByBillIdHandler(ISubscriptionQueryService subscriptionAssignmentQueryService)
        {
            _subscriptionAssignmentQueryService = subscriptionAssignmentQueryService;
            _subscriptionAssignmentQueryService.NotNull(nameof(subscriptionAssignmentQueryService));
        }

        public async Task<SubscriptionGetDto> Handle(SearchInput inputDto, CancellationToken cancellationToken)
        {
            SubscriptionGetDto customerInfo = await _subscriptionAssignmentQueryService.GetInfo(inputDto.Input);
            if (customerInfo == null)
            {
                throw new BaseException("شناسه قبض یافت نشد");
            }

            return customerInfo;
        }
    }
}
