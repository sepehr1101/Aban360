using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Implementations
{
    internal sealed class HandoverSummaryHandler : IHandoverSummaryHandler
    {
        private readonly IHandoverSummaryQueryService _handoverSummaryQueryService;
        private readonly IValidator<HandoverInputDto> _validator;
        public HandoverSummaryHandler(
            IHandoverSummaryQueryService handoverSummaryQueryService,
            IValidator<HandoverInputDto> validator)
        {
            _handoverSummaryQueryService = handoverSummaryQueryService;
            _handoverSummaryQueryService.NotNull(nameof(handoverSummaryQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<HandoverHeaderOutputDto, HandoverSummaryDataOutputDto>> Handle(HandoverInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<HandoverHeaderOutputDto, HandoverSummaryDataOutputDto> HandoverSummary = await _handoverSummaryQueryService.Get(input);
            return HandoverSummary;
        }
    }
}
