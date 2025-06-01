using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Implementations
{
    internal sealed class CustomerSearchAdvancedHandler : ICustomerSearchAdvancedHandler
    {
        private readonly ICustomerSearchAdvancedQueryService _customerSearchAdvancedQueryService;
        private readonly IValidator<CustomerSearchAdvancedInputDto> _validator;
        public CustomerSearchAdvancedHandler(
            ICustomerSearchAdvancedQueryService customerSearchAdvancedQueryService,
            IValidator<CustomerSearchAdvancedInputDto> validator)
        {
            _customerSearchAdvancedQueryService = customerSearchAdvancedQueryService;
            _customerSearchAdvancedQueryService.NotNull(nameof(customerSearchAdvancedQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ICollection<CustomerSearchOutputDto>> Handle(CustomerSearchAdvancedInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }
            ICollection<CustomerSearchOutputDto> customers = await _customerSearchAdvancedQueryService.GetInfo(input);
            return customers;
        }
    }
}
