using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using FluentValidation;
using System.Runtime.InteropServices;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Implementations
{
    internal sealed class CustomerMiniSearchHandler : ICustomerMiniSearchHandler
    {
        private readonly ICustomerMiniSearchQueryService _customerMiniSearchQueryService;
        private readonly IValidator<CustomerMiniSearchInputDto> _validator;
        public CustomerMiniSearchHandler(
            ICustomerMiniSearchQueryService customerMiniSearchQueryService,
            IValidator<CustomerMiniSearchInputDto> validator)
        {
            _customerMiniSearchQueryService = customerMiniSearchQueryService;
            _customerMiniSearchQueryService.NotNull(nameof(customerMiniSearchQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<CustomerMiniSearchHeaderOutputDto, CustomerMiniSearchDataOutputDto>> Handle(CustomerMiniSearchInputDto input, [Optional] CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input/*, cancellationToken*/);
            if (!validationResult.IsValid)
            {
                var messeState = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(messeState);
            }

            ReportOutput<CustomerMiniSearchHeaderOutputDto, CustomerMiniSearchDataOutputDto> result = await _customerMiniSearchQueryService.Get(input);
            return result;
        }
    }
}
