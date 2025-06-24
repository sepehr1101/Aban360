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
    internal sealed class ContractualCapacityHandler : IContractualCapacityHandler
    {
        private readonly IContractualCapacityQueryService _contractualCapacityQueryService;
        private readonly IValidator<ContractualCapacityInputDto> _validator;
        public ContractualCapacityHandler(
            IContractualCapacityQueryService contractualCapacityQueryService,
            IValidator<ContractualCapacityInputDto> validator)
        {
            _contractualCapacityQueryService = contractualCapacityQueryService;
            _contractualCapacityQueryService.NotNull(nameof(contractualCapacityQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<ContractualCapacityHeaderOutputDto, ContractualCapacityDataOutputDto>> Handle(ContractualCapacityInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<ContractualCapacityHeaderOutputDto, ContractualCapacityDataOutputDto> contractualCapacity = await _contractualCapacityQueryService.GetInfo(input);
            return contractualCapacity;
        }
    }
}
