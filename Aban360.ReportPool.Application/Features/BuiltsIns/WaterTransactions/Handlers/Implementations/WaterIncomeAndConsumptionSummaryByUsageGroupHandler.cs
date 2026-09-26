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
    internal sealed class WaterIncomeAndConsumptionSummaryByUsageGroupHandler : IWaterIncomeAndConsumptionSummaryByUsageGroupHandler
    {
        private readonly IWaterIncomeAndConsumptionSummaryByUsageGroupQueryService _waterIncomeAndConsumptionSummaryQueryService;
        private readonly IValidator<WaterIncomeAndConsumptionSummaryByUsageGroupInputDto> _validator;
        public WaterIncomeAndConsumptionSummaryByUsageGroupHandler(
            IWaterIncomeAndConsumptionSummaryByUsageGroupQueryService waterIncomeAndConsumptionSummaryQueryService,
            IValidator<WaterIncomeAndConsumptionSummaryByUsageGroupInputDto> validator)
        {
            _waterIncomeAndConsumptionSummaryQueryService = waterIncomeAndConsumptionSummaryQueryService;
            _waterIncomeAndConsumptionSummaryQueryService.NotNull(nameof(waterIncomeAndConsumptionSummaryQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<WaterIncomeAndConsumptionSummaryHeaderOutputDto, WaterIncomeAndConsumptionSummaryDataOutputDto>> Handle(WaterIncomeAndConsumptionSummaryByUsageGroupInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<WaterIncomeAndConsumptionSummaryHeaderOutputDto, WaterIncomeAndConsumptionSummaryDataOutputDto> waterIncomeAndConsumptionSummary = await _waterIncomeAndConsumptionSummaryQueryService.Get(input);
            return waterIncomeAndConsumptionSummary;
        }
    }
    internal sealed class WaterReturnSummaryByUsageGroupHandler : IWaterReturnSummaryByUsageGroupHandler
    {
        private readonly IWaterReturnSummaryByUsageGroupQueryService _waterReturnSummaryQueryService;
        private readonly IValidator<WaterReturnSummaryByUsageGroupInputDto> _validator;
        public WaterReturnSummaryByUsageGroupHandler(
            IWaterReturnSummaryByUsageGroupQueryService waterReturnSummaryQueryService,
            IValidator<WaterReturnSummaryByUsageGroupInputDto> validator)
        {
            _waterReturnSummaryQueryService = waterReturnSummaryQueryService;
            _waterReturnSummaryQueryService.NotNull(nameof(waterReturnSummaryQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<WaterReturnSummaryHeaderOutputDto, WaterReturnSummaryDataOutputDto>> Handle(WaterReturnSummaryByUsageGroupInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<WaterReturnSummaryHeaderOutputDto, WaterReturnSummaryDataOutputDto> WaterReturnSummary = await _waterReturnSummaryQueryService.Get(input);
            return WaterReturnSummary;
        }
    }
}
