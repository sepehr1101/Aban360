using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Implementations
{
    internal sealed class CustomerInfoGetByBillIdHandler : ICustomerInfoGetByBillIdHandler
    {
        private readonly ICustomerInfoQueryService _customerInfoQueryService;
        public CustomerInfoGetByBillIdHandler(ICustomerInfoQueryService customerInfoQueryService)
        {
            _customerInfoQueryService = customerInfoQueryService;
            _customerInfoQueryService.NotNull(nameof(customerInfoQueryService));
        }

        public async Task<CustomerInfoGetDto> Handle(SearchInput input, CancellationToken cancellationToken)
        {
            CustomerInfoGetDto customeInfo = await _customerInfoQueryService.Get(input.Input);
            return customeInfo;
        }
    }
}
