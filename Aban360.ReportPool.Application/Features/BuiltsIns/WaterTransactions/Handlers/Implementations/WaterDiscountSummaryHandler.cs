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
    internal sealed class WaterDiscountSummaryHandler : IWaterDiscountSummaryHandler
    {
        private readonly IWaterDiscountQueryService _waterDiscountSummaryQueryService;
        private readonly IValidator<WaterDiscountSummaryInputDto> _validator;
        public WaterDiscountSummaryHandler(
            IWaterDiscountQueryService waterDiscountSummaryQueryService,
            IValidator<WaterDiscountSummaryInputDto> validator)
        {
            _waterDiscountSummaryQueryService = waterDiscountSummaryQueryService;
            _waterDiscountSummaryQueryService.NotNull(nameof(waterDiscountSummaryQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<WaterDiscountSummaryHeaderOutputDto, WaterDiscountSummaryDataOutputDto>> Handle(WaterDiscountSummaryInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<WaterDiscountSummaryHeaderOutputDto, WaterDiscountSummaryDataOutputDto> result = await _waterDiscountSummaryQueryService.GetSummary(input);
            return result;
        }
    }
}
