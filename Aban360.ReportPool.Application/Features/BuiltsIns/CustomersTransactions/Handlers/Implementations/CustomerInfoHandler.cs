using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Implementations
{
    internal sealed class CustomerInfoHandler : ICustomerInfoHandler
    {
        private readonly ICustomerInfoQueryService _customerInfoQueryService;
        private readonly IValidator<CustomerInfoByZoneAndCustomerNumberInputDto> _validator;
        public CustomerInfoHandler(
            ICustomerInfoQueryService customerInfoQueryService,
            IValidator<CustomerInfoByZoneAndCustomerNumberInputDto> validator)
        {
            _customerInfoQueryService = customerInfoQueryService;
            _customerInfoQueryService.NotNull(nameof(customerInfoQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<CustomerInfoByBillIdOutputDto> Handle(SearchInput input, CancellationToken cancellationToken)
        {
            CustomerInfoByBillIdOutputDto customerInfo = await _customerInfoQueryService.Get(input.Input);
            return customerInfo;
        }
        public async Task<BillIdReppar> Handle(CustomerInfoByZoneAndCustomerNumberInputDto input, CancellationToken cancellationToken)
        {
            var validatioResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validatioResult.IsValid)
            {
                var message = string.Join(", ", validatioResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
            return await _customerInfoQueryService.Get(input);
        }
    }
}
