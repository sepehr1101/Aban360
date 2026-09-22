using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Implementations
{
    internal sealed class SewageWaterRequestSummaryByZoneWithStringCodeHandler : ISewageWaterRequestSummaryByZoneWithStringCodeHandler
    {
        private readonly ISewageWaterRequestSummaryByZoneWithStringCodeQueryService _sewageWaterRequestSummaryByZoneQuery;
        private readonly IValidator<SewageWaterRequestWithStringCodeInputDto> _validator;
        public SewageWaterRequestSummaryByZoneWithStringCodeHandler(
            ISewageWaterRequestSummaryByZoneWithStringCodeQueryService sewageWaterRequestSummaryByZoneQuery,
            IValidator<SewageWaterRequestWithStringCodeInputDto> validator)
        {
            _sewageWaterRequestSummaryByZoneQuery = sewageWaterRequestSummaryByZoneQuery;
            _sewageWaterRequestSummaryByZoneQuery.NotNull(nameof(sewageWaterRequestSummaryByZoneQuery));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<SewageWaterRequestHeaderOutputDto, SewageWaterRequestSummaryWithStringCodeDataOutputDto>> Handle(SewageWaterRequestWithStringCodeInputDto input, CancellationToken cancellationToken)
        {
            await Validate(input,cancellationToken);

            var result = await _sewageWaterRequestSummaryByZoneQuery.Get(input);
            return result;
        }
        private async Task Validate(SewageWaterRequestWithStringCodeInputDto input, CancellationToken cancellationToken)
        {
            await InputValidate(input, cancellationToken);
            if (input.IsZone && input.ZoneIds.Where(z => z > 140000).Any())
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidVillageIdByZoneSelect);
            }
            if (!input.IsZone && input.ZoneIds.Where(z => z < 140000).Any())
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidZoneIdByVillageSelect);
            }
        }
        private async Task InputValidate(SewageWaterRequestWithStringCodeInputDto input, CancellationToken cancellationToken)
        {
            var validatioResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validatioResult.IsValid)
            {
                var message = string.Join(", ", validatioResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
    }
}
