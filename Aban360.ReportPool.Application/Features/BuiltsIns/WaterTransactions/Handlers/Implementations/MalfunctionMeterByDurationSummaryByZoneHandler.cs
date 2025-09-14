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
    internal sealed class MalfunctionMeterByDurationSummaryByZoneHandler : IMalfunctionMeterByDurationSummaryByZoneHandler
    {
        private readonly IMalfunctionMeterByDurationSummaryByZoneQueryService _malfunctionMeterByDurationSummaryByZoneQueryService;
        private readonly IValidator<MalfunctionMeterByDurationInputDto> _validator;
        public MalfunctionMeterByDurationSummaryByZoneHandler(
            IMalfunctionMeterByDurationSummaryByZoneQueryService malfunctionMeterByDurationSummaryByZoneQueryService,
            IValidator<MalfunctionMeterByDurationInputDto> validator)
        {
            _malfunctionMeterByDurationSummaryByZoneQueryService = malfunctionMeterByDurationSummaryByZoneQueryService;
            _malfunctionMeterByDurationSummaryByZoneQueryService.NotNull(nameof(malfunctionMeterByDurationSummaryByZoneQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationSummaryByZoneDataOutputDto>> Handle(MalfunctionMeterByDurationInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationSummaryByZoneDataOutputDto> malfunctionMeterByDurationSummaryByZone = await _malfunctionMeterByDurationSummaryByZoneQueryService.Get(input);

            return malfunctionMeterByDurationSummaryByZone;
        }
    }
}
