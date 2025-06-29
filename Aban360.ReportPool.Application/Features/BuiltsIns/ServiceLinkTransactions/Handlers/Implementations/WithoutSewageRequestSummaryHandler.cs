using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Implementations
{
    internal sealed class WithoutSewageRequestSummaryHandler : IWithoutSewageRequestSummaryHandler
    {
        private readonly IWithoutSewageRequestSummaryQueryService _withoutSewageRequestSummaryQuery;
        private readonly IValidator<WithoutSewageRequestInputDto> _validator;
        public WithoutSewageRequestSummaryHandler(
            IWithoutSewageRequestSummaryQueryService withoutSewageRequestSummaryQuery,
            IValidator<WithoutSewageRequestInputDto> validator)
        {
            _withoutSewageRequestSummaryQuery = withoutSewageRequestSummaryQuery;
            _withoutSewageRequestSummaryQuery.NotNull(nameof(withoutSewageRequestSummaryQuery));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<WithoutSewageRequestHeaderOutputDto, WithoutSewageRequestSummaryDataOutputDto>> Handle(WithoutSewageRequestInputDto input, CancellationToken cancellationToken)
        {
            var validatioResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validatioResult.IsValid)
            {
                var message = string.Join(", ", validatioResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var result = await _withoutSewageRequestSummaryQuery.Get(input);
            return result;
        }
    }
}
