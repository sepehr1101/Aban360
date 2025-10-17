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
    internal sealed class WaterIncomeAndConsumptionDetailHandler : IWaterIncomeAndConsumptionDetailHandler
    {
        private readonly IWaterIncomeAndConsumptionDetailQueryService _waterIncomeAndConsumptionDetailQueryService;
        private readonly IValidator<WaterIncomeAndConsumptionDetailInputDto> _validator;
        public WaterIncomeAndConsumptionDetailHandler(
            IWaterIncomeAndConsumptionDetailQueryService waterIncomeAndConsumptionDetailQueryService,
            IValidator<WaterIncomeAndConsumptionDetailInputDto> validator)
        {
            _waterIncomeAndConsumptionDetailQueryService = waterIncomeAndConsumptionDetailQueryService;
            _waterIncomeAndConsumptionDetailQueryService.NotNull(nameof(waterIncomeAndConsumptionDetailQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<WaterIncomeAndConsumptionDetailHeaderOutputDto, WaterIncomeAndConsumptionDetailDataOutputDto>> Handle(WaterIncomeAndConsumptionDetailInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<WaterIncomeAndConsumptionDetailHeaderOutputDto, WaterIncomeAndConsumptionDetailDataOutputDto> waterIncomeAndConsumptionDetail = await _waterIncomeAndConsumptionDetailQueryService.Get(input);
            return waterIncomeAndConsumptionDetail;
        }
    }
}
