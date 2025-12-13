using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Queries.Implementations
{
    internal sealed class ManualBillGetHandler : IManualBillGetHandler
    {
        private readonly IBedBesQueryService _bedBesQueryService;
        private readonly IValidator<ManualBillInputDto> _validator;
        public ManualBillGetHandler(
            IBedBesQueryService bedBesQueryService,
            IValidator<ManualBillInputDto> validator)
        {
            _bedBesQueryService = bedBesQueryService;
            _bedBesQueryService.NotNull(nameof(bedBesQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<ManualBillHeaderOutputDto, ManualBillDataOutputDto>> Handle(ManualBillInputDto inputDto, CancellationToken cancellationToken)
        {
            await Validation(inputDto, cancellationToken);
            ReportOutput<ManualBillHeaderOutputDto, ManualBillDataOutputDto> result = await _bedBesQueryService.Get(inputDto);
            return result;
        }
        private async Task Validation(ManualBillInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                string message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
    }
}