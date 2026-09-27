using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Implementations
{
    internal sealed class WaterIncomeDiscountDetailHandler : IWaterIncomeDiscountDetailHandler
    {
        private readonly IWaterIncomeDiscountDetailQueryService _waterIncomeDiscountDetailQueryService;
        private readonly IValidator<WaterIncomeDiscountDetailInputDto> _validator;
        public WaterIncomeDiscountDetailHandler(
            IWaterIncomeDiscountDetailQueryService waterIncomeDiscountDetailQueryService,
            IValidator<WaterIncomeDiscountDetailInputDto> validator)
        {
            _waterIncomeDiscountDetailQueryService = waterIncomeDiscountDetailQueryService;
            _waterIncomeDiscountDetailQueryService.NotNull(nameof(waterIncomeDiscountDetailQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<WaterIncomeDiscountDetailHeaderOutputDto, WaterIncomeDiscountDetailDataOutputDto>> Handle(WaterIncomeDiscountDetailInputDto input, CancellationToken cancellationToken)
        {
            await Validation(input, cancellationToken);

            ReportOutput<WaterIncomeDiscountDetailHeaderOutputDto, WaterIncomeDiscountDetailDataOutputDto> waterIncomeDiscountDetail = await _waterIncomeDiscountDetailQueryService.Get(input);
            return waterIncomeDiscountDetail;
        }
        private async Task Validation(WaterIncomeDiscountDetailInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            bool hasReadingNumberRange = !string.IsNullOrWhiteSpace(input.FromReadingNumber) && !string.IsNullOrWhiteSpace(input.ToReadingNumber);
            bool hasMultiplierZoneId = input.ZoneIds.Skip(1).Any();

            if (hasReadingNumberRange && hasMultiplierZoneId)
            {
                throw new InvalidDataException(ExceptionLiterals.InvalidZoneIdMoreThan1);
            }
        }
    }
}
