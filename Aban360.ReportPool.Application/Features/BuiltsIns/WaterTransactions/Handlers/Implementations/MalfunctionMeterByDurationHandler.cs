using Aban360.Common.Excel;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Timing;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Implementations
{
    internal sealed class MalfunctionMeterByDurationHandler : IMalfunctionMeterByDurationHandler
    {
        private readonly IMalfunctionMeterByDurationQueryService _malfunctionMeterByDurationQueryService;
        private readonly IValidator<MalfunctionMeterByDurationInputDto> _validator;
        public MalfunctionMeterByDurationHandler(
            IMalfunctionMeterByDurationQueryService malfunctionMeterByDurationQueryService,
            IValidator<MalfunctionMeterByDurationInputDto> validator)
        {
            _malfunctionMeterByDurationQueryService = malfunctionMeterByDurationQueryService;
            _malfunctionMeterByDurationQueryService.NotNull(nameof(malfunctionMeterByDurationQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationDataOutputDto>> Handle(MalfunctionMeterByDurationInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationDataOutputDto> malfunctionMeterByDuration = await _malfunctionMeterByDurationQueryService.Get(input);
            malfunctionMeterByDuration.ReportData.ForEach(data =>
            {
                data.MeterLife = CalculationDistanceDate.CalcDistance(data.LastChangeDateJalali);
            });
            
            return malfunctionMeterByDuration;
        }
    }
}
