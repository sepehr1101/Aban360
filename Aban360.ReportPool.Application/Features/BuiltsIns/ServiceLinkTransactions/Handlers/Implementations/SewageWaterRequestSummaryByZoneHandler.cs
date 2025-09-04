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
    internal sealed class SewageWaterRequestSummaryByZoneHandler : ISewageWaterRequestSummaryByZoneHandler
    {
        private readonly ISewageWaterRequestSummaryByZoneQueryService _sewageWaterRequestSummaryByZoneQuery;
        private readonly IValidator<SewageWaterRequestInputDto> _validator;
        public SewageWaterRequestSummaryByZoneHandler(
            ISewageWaterRequestSummaryByZoneQueryService sewageWaterRequestSummaryByZoneQuery,
            IValidator<SewageWaterRequestInputDto> validator)
        {
            _sewageWaterRequestSummaryByZoneQuery = sewageWaterRequestSummaryByZoneQuery;
            _sewageWaterRequestSummaryByZoneQuery.NotNull(nameof(sewageWaterRequestSummaryByZoneQuery));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<SewageWaterRequestHeaderOutputDto, SewageWaterRequestSummaryByZoneDataOutputDto>> Handle(SewageWaterRequestInputDto input, CancellationToken cancellationToken)
        {
            var validatioResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validatioResult.IsValid)
            {
                var message = string.Join(", ", validatioResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var result = await _sewageWaterRequestSummaryByZoneQuery.Get(input);
            return result;
        }
    }
}
