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
    internal sealed class SewageWaterInstallationDepartmentDetailHandler : ISewageWaterInstallationDepartmentDetailHandler
    {
        private readonly ISewageWaterInstallationDepartmentDetailQueryService _sewageWaterInstallationDepartmentDetailQuery;
        private readonly IValidator<SewageWaterInstallationInputDto> _validator;
        public SewageWaterInstallationDepartmentDetailHandler(
            ISewageWaterInstallationDepartmentDetailQueryService sewageWaterInstallationDepartmentDetailQuery,
            IValidator<SewageWaterInstallationInputDto> validator)
        {
            _sewageWaterInstallationDepartmentDetailQuery = sewageWaterInstallationDepartmentDetailQuery;
            _sewageWaterInstallationDepartmentDetailQuery.NotNull(nameof(sewageWaterInstallationDepartmentDetailQuery));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<SewageWaterInstallationHeaderOutputDto, SewageWaterInstallationDetailDataOutputDto>> Handle(SewageWaterInstallationInputDto input, CancellationToken cancellationToken)
        {
            var validatioResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validatioResult.IsValid)
            {
                var message = string.Join(", ", validatioResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            var result = await _sewageWaterInstallationDepartmentDetailQuery.Get(input);
            return result;
        }
    }
}
