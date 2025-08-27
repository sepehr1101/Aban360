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
    internal sealed class WaterCalculationDetailsHandler : IWaterCalculationDetailsHandler
    {
        private readonly IWaterCalculationDetailsQueryService _calculationDetailsQueryService;
        private readonly IValidator<WaterCalculationDetailsInputDto> _validator;
        public WaterCalculationDetailsHandler(
            IWaterCalculationDetailsQueryService calculationDetailsQueryService,
            IValidator<WaterCalculationDetailsInputDto> validator)
        {
            _calculationDetailsQueryService = calculationDetailsQueryService;
            _calculationDetailsQueryService.NotNull(nameof(calculationDetailsQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<WaterCalculationDetailsHeaderOutputDto, WaterCalculationDetailsDataOutputDto>> Handle(WaterCalculationDetailsInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<WaterCalculationDetailsHeaderOutputDto, WaterCalculationDetailsDataOutputDto> calculationDetails = await _calculationDetailsQueryService.GetInfo(input);
            return calculationDetails;
        }
    }
}
