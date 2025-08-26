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
    internal sealed class MalfunctionToChangeDetailHandler : IMalfunctionToChangeDetailHandler
    {
        private readonly IMalfunctionToChangeDetailQueryService _malfunctionToChangeQueryService;
        private readonly IValidator<MalfunctionToChangeInputDto> _validator;
        public MalfunctionToChangeDetailHandler(
            IMalfunctionToChangeDetailQueryService malfunctionToChangeQueryService,
            IValidator<MalfunctionToChangeInputDto> validator)
        {
            _malfunctionToChangeQueryService = malfunctionToChangeQueryService;
            _malfunctionToChangeQueryService.NotNull(nameof(malfunctionToChangeQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<MalfunctionToChangeHeaderOutputDto, MalfunctionToChangeDetailDataOutputDto>> Handle(MalfunctionToChangeInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<MalfunctionToChangeHeaderOutputDto, MalfunctionToChangeDetailDataOutputDto> malfunctionToChange = await _malfunctionToChangeQueryService.Get(input);
            malfunctionToChange.ReportData.ForEach(meter =>
            {
                int duration = int.Parse(CalculationDistanceDate.CalcDistance(meter.LatestMalfunctinDateJalali, meter.ChangeDateJalali));
                meter.Duration = CalculationDistanceDate.ConvertDaysToDate(duration);
            });
            
            return malfunctionToChange;
        }
    }
}
