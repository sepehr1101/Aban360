using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Implementations
{
    internal sealed class WithoutSewageRequestSummaryByZoneHandler : IWithoutSewageRequestSummaryByZoneHandler
    {
        private readonly IWithoutSewageRequestSummaryByZoneQueryService _withoutSewageRequestSummaryByZoneQuery;
        private readonly IValidator<WithoutSewageRequestInputDto> _validator;
        public WithoutSewageRequestSummaryByZoneHandler(
            IWithoutSewageRequestSummaryByZoneQueryService withoutSewageRequestSummaryByZoneQuery,
            IValidator<WithoutSewageRequestInputDto> validator)
        {
            _withoutSewageRequestSummaryByZoneQuery = withoutSewageRequestSummaryByZoneQuery;
            _withoutSewageRequestSummaryByZoneQuery.NotNull(nameof(withoutSewageRequestSummaryByZoneQuery));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<WithoutSewageRequestHeaderOutputDto, WithoutSewageRequestSummaryDataOutputDto>> Handle(WithoutSewageRequestInputDto input, CancellationToken cancellationToken)
        {
            var validatioResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validatioResult.IsValid)
            {
                var message = string.Join(", ", validatioResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            var result = await _withoutSewageRequestSummaryByZoneQuery.Get(input);
            return result;
        }
    }
}
