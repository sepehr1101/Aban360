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
    internal sealed class ConsumptionAverageAnalysisHandler : IConsumptionAverageAnalysisHandler
    {
        private readonly IConsumptionAverageAnalysisQueryService _lowWorkingMeter;
        private readonly IValidator<ConsumptionAverageAnalysisInputDto> _validator;
        public ConsumptionAverageAnalysisHandler(
            IConsumptionAverageAnalysisQueryService lowWorkingMeter,
            IValidator<ConsumptionAverageAnalysisInputDto> validator)
        {
            _lowWorkingMeter = lowWorkingMeter;
            _lowWorkingMeter.NotNull(nameof(lowWorkingMeter));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<ConsumptionAverageAnalysisHeaderOutputDto, ConsumptionAverageAnalysisDataOutputDto>> Handle(ConsumptionAverageAnalysisInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<ConsumptionAverageAnalysisHeaderOutputDto, ConsumptionAverageAnalysisDataOutputDto> result = await _lowWorkingMeter.Get(input);

            return result;
        }
    }
}
