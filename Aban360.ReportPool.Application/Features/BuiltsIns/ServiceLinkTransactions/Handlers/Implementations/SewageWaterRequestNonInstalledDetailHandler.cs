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
    internal sealed class SewageWaterRequestNonInstalledDetailHandler : ISewageWaterRequestNonInstalledDetailHandler
    {
        private readonly ISewageWaterRequestNonInstalledDetailQueryService _sewageWaterRequestNonInstalledDetailQuery;
        private readonly IValidator<SewageWaterRequestNonInstalledInputDto> _validator;
        public SewageWaterRequestNonInstalledDetailHandler(
            ISewageWaterRequestNonInstalledDetailQueryService sewageWaterRequestNonInstalledDetailQuery,
            IValidator<SewageWaterRequestNonInstalledInputDto> validator)
        {
            _sewageWaterRequestNonInstalledDetailQuery = sewageWaterRequestNonInstalledDetailQuery;
            _sewageWaterRequestNonInstalledDetailQuery.NotNull(nameof(sewageWaterRequestNonInstalledDetailQuery));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<SewageWaterRequestNonInstalledHeaderOutputDto, SewageWaterRequestNonInstalledDetailDataOutputDto>> Handle(SewageWaterRequestNonInstalledInputDto input, CancellationToken cancellationToken)
        {
            var validatioResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validatioResult.IsValid)
            {
                var message = string.Join(", ", validatioResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var result = await _sewageWaterRequestNonInstalledDetailQuery.Get(input);
            return result;
        }
    }
}