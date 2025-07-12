using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Implementations
{
    internal sealed class CustomerInfoByBIllIdHandler : ICustomerInfoByBIllIdHandler
    {
        private readonly ICustomerInfoByBillIdQueryService _customerInfoByBillIdQueryService;
        public CustomerInfoByBIllIdHandler(ICustomerInfoByBillIdQueryService customerInfoByBillIdQueryService)
        {
            _customerInfoByBillIdQueryService = customerInfoByBillIdQueryService;
            _customerInfoByBillIdQueryService.NotNull(nameof(customerInfoByBillIdQueryService));
        }

        public async Task<CustomerInfoByBillIdOutputDto> Handle(SearchInput input, CancellationToken cancellationToken)
        {
            CustomerInfoByBillIdOutputDto customerInfo = await _customerInfoByBillIdQueryService.Get(input.Input);
            return customerInfo;
        }
    }
}
