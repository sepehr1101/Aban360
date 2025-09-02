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
    internal sealed class WaterIncomeAndConsumptionSummaryHandler : IWaterIncomeAndConsumptionSummaryHandler
    {
        private readonly IWaterIncomeAndConsumptionSummaryQueryService _waterIncomeAndConsumptionSummaryQueryService;
        private readonly IValidator<WaterIncomeAndConsumptionSummaryInputDto> _validator;
        public WaterIncomeAndConsumptionSummaryHandler(
            IWaterIncomeAndConsumptionSummaryQueryService waterIncomeAndConsumptionSummaryQueryService,
            IValidator<WaterIncomeAndConsumptionSummaryInputDto> validator)
        {
            _waterIncomeAndConsumptionSummaryQueryService = waterIncomeAndConsumptionSummaryQueryService;
            _waterIncomeAndConsumptionSummaryQueryService.NotNull(nameof(waterIncomeAndConsumptionSummaryQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<WaterIncomeAndConsumptionSummaryHeaderOutputDto, WaterIncomeAndConsumptionSummaryDataOutputDto>> Handle(WaterIncomeAndConsumptionSummaryInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<WaterIncomeAndConsumptionSummaryHeaderOutputDto, WaterIncomeAndConsumptionSummaryDataOutputDto> waterIncomeAndConsumptionSummary = await _waterIncomeAndConsumptionSummaryQueryService.Get(input);
            return waterIncomeAndConsumptionSummary;
        }
    }
}
