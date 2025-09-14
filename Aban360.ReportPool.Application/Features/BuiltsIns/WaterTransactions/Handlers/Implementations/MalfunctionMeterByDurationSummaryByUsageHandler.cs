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
    internal sealed class MalfunctionMeterByDurationSummaryByUsageHandler : IMalfunctionMeterByDurationSummaryByUsageHandler
    {
        private readonly IMalfunctionMeterByDurationSummaryByUsageQueryService _malfunctionMeterByDurationSummaryByUsageQueryService;
        private readonly IValidator<MalfunctionMeterByDurationInputDto> _validator;
        public MalfunctionMeterByDurationSummaryByUsageHandler(
            IMalfunctionMeterByDurationSummaryByUsageQueryService malfunctionMeterByDurationSummaryByUsageQueryService,
            IValidator<MalfunctionMeterByDurationInputDto> validator)
        {
            _malfunctionMeterByDurationSummaryByUsageQueryService = malfunctionMeterByDurationSummaryByUsageQueryService;
            _malfunctionMeterByDurationSummaryByUsageQueryService.NotNull(nameof(malfunctionMeterByDurationSummaryByUsageQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationSummaryDataOutputDto>> Handle(MalfunctionMeterByDurationInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationSummaryDataOutputDto> malfunctionMeterByDurationSummaryByUsage = await _malfunctionMeterByDurationSummaryByUsageQueryService.Get(input);

            return malfunctionMeterByDurationSummaryByUsage;
        }
    }
}
