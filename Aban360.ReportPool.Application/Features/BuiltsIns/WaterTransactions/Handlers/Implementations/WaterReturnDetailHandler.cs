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
    internal sealed class WaterReturnDetailHandler : IWaterReturnDetailHandler
    {
        private readonly IWaterReturnDetailQueryService _waterReturnDetailQueryService;
        private readonly IValidator<WaterReturnDetailInputDto> _validator;
        public WaterReturnDetailHandler(
            IWaterReturnDetailQueryService waterReturnDetailQueryService,
            IValidator<WaterReturnDetailInputDto> validator)
        {
            _waterReturnDetailQueryService = waterReturnDetailQueryService;
            _waterReturnDetailQueryService.NotNull(nameof(waterReturnDetailQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<WaterReturnDetailHeaderOutputDto, WaterReturnDetailDataOutputDto>> Handle(WaterReturnDetailInputDto input, CancellationToken cancellationToken)
        {
            await Validation(input, cancellationToken);

            ReportOutput<WaterReturnDetailHeaderOutputDto, WaterReturnDetailDataOutputDto> WaterReturnDetail = await _waterReturnDetailQueryService.Get(input);
            return WaterReturnDetail;
        }
        private async Task Validation(WaterReturnDetailInputDto input, CancellationToken cancellationToken)
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
