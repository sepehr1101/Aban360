using Aban360.Common.Excel;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Implementations
{
    internal sealed class MalfunctionMeterHandler : IMalfunctionMeterHandler
    {
        private readonly IMalfunctionMeterQueryService _malfunctionMeterQueryService;
        private readonly IValidator<MalfunctionMeterInputDto> _validator;
        public MalfunctionMeterHandler(
            IMalfunctionMeterQueryService malfunctionMeterQueryService,
            IValidator<MalfunctionMeterInputDto> validator)
        {
            _malfunctionMeterQueryService = malfunctionMeterQueryService;
            _malfunctionMeterQueryService.NotNull(nameof(malfunctionMeterQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<MalfunctionMeterHeaderOutputDto, MalfunctionMeterDataOutputDto>> Handle(MalfunctionMeterInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<MalfunctionMeterHeaderOutputDto, MalfunctionMeterDataOutputDto> malfunctionMeter = await _malfunctionMeterQueryService.Get(input);
            return malfunctionMeter;
        }
    }
}
