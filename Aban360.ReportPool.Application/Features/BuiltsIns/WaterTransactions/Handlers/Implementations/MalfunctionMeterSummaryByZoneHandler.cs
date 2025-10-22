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
    internal sealed class MalfunctionMeterSummaryByZoneHandler : IMalfunctionMeterSummaryByZoneHandler
    {
        private readonly IMalfunctionMeterSummaryByZoneQueryService _malfunctionMeterSummaryByZoneQueryService;
        private readonly IValidator<MalfunctionMeterInputDto> _validator;
        public MalfunctionMeterSummaryByZoneHandler(
            IMalfunctionMeterSummaryByZoneQueryService malfunctionMeterSummaryByZoneQueryService,
            IValidator<MalfunctionMeterInputDto> validator)
        {
            _malfunctionMeterSummaryByZoneQueryService = malfunctionMeterSummaryByZoneQueryService;
            _malfunctionMeterSummaryByZoneQueryService.NotNull(nameof(malfunctionMeterSummaryByZoneQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<MalfunctionMeterSummaryHeaderOutputDto, MalfunctionMeterSummaryDataOutputDto>> Handle(MalfunctionMeterInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<MalfunctionMeterSummaryHeaderOutputDto, MalfunctionMeterSummaryDataOutputDto> malfunctionMeterSummaryByZone = await _malfunctionMeterSummaryByZoneQueryService.Get(input);
            malfunctionMeterSummaryByZone.ReportHeader.ConsumptionAverage = (float)Math.Round(malfunctionMeterSummaryByZone.ReportHeader.ConsumptionAverage, 3);

            return malfunctionMeterSummaryByZone;
        }
    }
}
