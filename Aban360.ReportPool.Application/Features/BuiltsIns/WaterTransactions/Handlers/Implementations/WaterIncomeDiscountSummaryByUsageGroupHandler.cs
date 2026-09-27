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
    internal sealed class WaterIncomeDiscountSummaryByUsageGroupHandler : IWaterIncomeDiscountSummaryByUsageGroupHandler
    {
        private readonly IWaterIncomeDiscountSummaryByUsageGroupQueryService _waterIncomeDiscountSummaryQueryService;
        private readonly IValidator<WaterIncomeDiscountSummaryByUsageGroupInputDto> _validator;
        public WaterIncomeDiscountSummaryByUsageGroupHandler(
            IWaterIncomeDiscountSummaryByUsageGroupQueryService waterIncomeDiscountSummaryQueryService,
            IValidator<WaterIncomeDiscountSummaryByUsageGroupInputDto> validator)
        {
            _waterIncomeDiscountSummaryQueryService = waterIncomeDiscountSummaryQueryService;
            _waterIncomeDiscountSummaryQueryService.NotNull(nameof(waterIncomeDiscountSummaryQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<WaterIncomeDiscountSummaryHeaderOutputDto, WaterIncomeDiscountSummaryDataOutputDto>> Handle(WaterIncomeDiscountSummaryByUsageGroupInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<WaterIncomeDiscountSummaryHeaderOutputDto, WaterIncomeDiscountSummaryDataOutputDto> waterIncomeDiscountSummary = await _waterIncomeDiscountSummaryQueryService.Get(input);
            return waterIncomeDiscountSummary;
        }
    }
}
