using Aban360.Common.Excel;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Implementations
{
    internal sealed class CustomerSearchHandler : ICustomerSearchHandler
    {
        private readonly ICustomerSearchQueryService _customerSearchQueryService;
        private readonly IValidator<CustomerSearchInputDto> _validator;
        public CustomerSearchHandler(
            ICustomerSearchQueryService customerSearchQueryService,
            IValidator<CustomerSearchInputDto> validator)
        {
            _customerSearchQueryService = customerSearchQueryService;
            _customerSearchQueryService.NotNull(nameof(customerSearchQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<CustomerSearchHeaderOutputDto, CustomerSearchDataOutputDto>> Handle(CustomerSearchInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<CustomerSearchHeaderOutputDto, CustomerSearchDataOutputDto> customers = await _customerSearchQueryService.GetInfo(input);
            return customers;
        }
    }
}
