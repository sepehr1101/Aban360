using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Implementations
{
    internal sealed class CustomerValidateByBillIdHandler : ICustomerValidateByBillIdHandler
    {
        private readonly ICustomerInfoQueryService _customerInfoQueryService;
        private readonly IValidator<CustomerInfoByZoneAndCustomerNumberInputDto> _validator;
        public CustomerValidateByBillIdHandler(
            ICustomerInfoQueryService customerInfoQueryService,
            IValidator<CustomerInfoByZoneAndCustomerNumberInputDto> validator)
        {
            _customerInfoQueryService = customerInfoQueryService;
            _customerInfoQueryService.NotNull(nameof(customerInfoQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<CustomerBillIdValidateDto> Handle(SearchInput input, CancellationToken cancellationToken)
        {
            CustomerInfoByBillIdOutputDto? customerInfo = await _customerInfoQueryService.Get(input.Input, true);
            bool isValidBillId = customerInfo is null ? false : true;
            return new CustomerBillIdValidateDto(input.Input, isValidBillId);
        }
    }
}
