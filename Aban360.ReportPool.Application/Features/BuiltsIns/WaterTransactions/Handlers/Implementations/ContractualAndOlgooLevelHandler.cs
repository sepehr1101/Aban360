using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Implementations
{
    internal sealed class ContractualAndOlgooLevelHandler : IContractualAndOlgooLevelHandler
    {
        private readonly IContractualAndOlgooLevelQueryService _contractualAndOlgooLevelQueryService;
        private readonly IValidator<ContractualAndOlgooLevelInputDto> _validator;
        public ContractualAndOlgooLevelHandler(
            IContractualAndOlgooLevelQueryService ConsumptionAndOlgooLevelQueryService,
            IValidator<ContractualAndOlgooLevelInputDto> validator)
        {
            _contractualAndOlgooLevelQueryService = ConsumptionAndOlgooLevelQueryService;
            _contractualAndOlgooLevelQueryService.NotNull(nameof(ConsumptionAndOlgooLevelQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<ContractualAndOlgooLevelHeaderOutputDto, ContractualAndOlgooLevelDataOutputDto>> Handle(ContractualAndOlgooLevelInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<ContractualAndOlgooLevelHeaderOutputDto, ContractualAndOlgooLevelDataOutputDto> contractualAndOlgooLevel = await _contractualAndOlgooLevelQueryService.Get(input);

            return contractualAndOlgooLevel;
        }
    }
}
