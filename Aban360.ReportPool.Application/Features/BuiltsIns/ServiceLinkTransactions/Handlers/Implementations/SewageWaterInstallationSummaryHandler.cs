using Aban360.Common.BaseEntities;
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
    internal sealed class SewageWaterInstallationSummaryHandler : ISewageWaterInstallationSummaryHandler
    {
        private readonly ISewageWaterInstallationSummaryQueryService _sewageWaterInstallationSummaryQuery;
        private readonly IValidator<SewageWaterInstallationInputDto> _validator;
        public SewageWaterInstallationSummaryHandler(
            ISewageWaterInstallationSummaryQueryService sewageWaterInstallationSummaryQuery,
            IValidator<SewageWaterInstallationInputDto> validator)
        {
            _sewageWaterInstallationSummaryQuery = sewageWaterInstallationSummaryQuery;
            _sewageWaterInstallationSummaryQuery.NotNull(nameof(sewageWaterInstallationSummaryQuery));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<SewageWaterInstallationHeaderOutputDto, SewageWaterInstallationSummaryDataOutputDto>> Handle(SewageWaterInstallationInputDto input, CancellationToken cancellationToken)
        {
            var validatioResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validatioResult.IsValid)
            {
                var message = string.Join(", ", validatioResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            var result = await _sewageWaterInstallationSummaryQuery.Get(input);
            return result;
        }
    }
}
