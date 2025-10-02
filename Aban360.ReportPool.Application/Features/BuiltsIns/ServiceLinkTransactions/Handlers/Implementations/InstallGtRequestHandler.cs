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
    internal sealed class InstallGtRequestHandler : IInstallGtRequestHandler
    {
        private readonly IInstallGtRequestQueryService _installGtRequestQueryService;
        private readonly IValidator<InstallGtRequestInputDto> _validator;
        public InstallGtRequestHandler(IInstallGtRequestQueryService installGtRequestQueryService, IValidator<InstallGtRequestInputDto> validator)
        {
            _installGtRequestQueryService = installGtRequestQueryService;
            _installGtRequestQueryService.NotNull(nameof(installGtRequestQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<InstallGtRequestHeaderOutputDto, InstallGtRequestDataOutputDto>> Handle(InstallGtRequestInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<InstallGtRequestHeaderOutputDto, InstallGtRequestDataOutputDto> result = await _installGtRequestQueryService.GetInfo(input);
            return result;
        }
    }
}
