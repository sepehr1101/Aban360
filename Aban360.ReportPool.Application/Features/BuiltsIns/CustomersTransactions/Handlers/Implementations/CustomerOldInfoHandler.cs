﻿using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Implementations
{
    internal sealed class CustomerOldInfoHandler : ICustomerOldInfoHandler
    {
        private readonly ICustomerOldInfoQueryService _customerOldInfoQueryService;
        private readonly IValidator<CustomerOldInfoInputDto> _validator;
        public CustomerOldInfoHandler(
            ICustomerOldInfoQueryService customerOldInfoQueryService,
            IValidator<CustomerOldInfoInputDto> validator)
        {
            _customerOldInfoQueryService = customerOldInfoQueryService;
            _customerOldInfoQueryService.NotNull(nameof(customerOldInfoQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<CustomerOldInfoOutputDto> Handle(CustomerOldInfoInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            CustomerOldInfoOutputDto customerOldInfo = await _customerOldInfoQueryService.GetInfo(input);
            return customerOldInfo;
        }
    }
}
