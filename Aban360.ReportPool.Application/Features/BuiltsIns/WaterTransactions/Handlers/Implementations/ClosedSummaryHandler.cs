using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts
{
    internal sealed class ClosedSummaryHandler : IClosedSummaryHandler
    {
        private readonly IClosedSummaryQueryService _closedService;
        private readonly IValidator<ClosedInputDto> _validator;
        public ClosedSummaryHandler(
            IClosedSummaryQueryService closedService,
            IValidator<ClosedInputDto> validator)
        {
            _closedService = closedService;
            _closedService.NotNull(nameof(closedService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<ClosedHeaderOutputDto, ClosedSummaryDataOutputDto>> Handle(ClosedInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ReportOutput<ClosedHeaderOutputDto, ClosedSummaryDataOutputDto> result = await _closedService.Get(input);

            return result;
        }
    }
}
