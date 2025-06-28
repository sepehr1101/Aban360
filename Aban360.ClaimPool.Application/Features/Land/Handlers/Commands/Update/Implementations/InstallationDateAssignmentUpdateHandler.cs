using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    internal sealed class InstallationDateAssignmentUpdateHandler : IInstallationDateAssignmentUpdateHandler
    {
        private readonly IInstallationDateAssignmentQueryService _InstallationDateAssignmentQueryService;
        private readonly IInstallationDateAssignmentCommandService _InstallationDateAssignmentCommandService;
        public InstallationDateAssignmentUpdateHandler(
            IInstallationDateAssignmentQueryService installationDateAssignmentQueryService,
            IInstallationDateAssignmentCommandService installationDateAssignmentCommandService)
        {
            _InstallationDateAssignmentQueryService = installationDateAssignmentQueryService;
            _InstallationDateAssignmentQueryService.NotNull(nameof(installationDateAssignmentQueryService));

            _InstallationDateAssignmentCommandService = installationDateAssignmentCommandService;
            _InstallationDateAssignmentCommandService.NotNull(nameof(installationDateAssignmentCommandService));
        }

        public async Task Handle(InstallationDateAssignmentUpdateDto updateDto, CancellationToken cancellationToken)
        {
            InstallationDateAssignmentGetDto previousInstallationDate = await _InstallationDateAssignmentQueryService.Get(updateDto.BillId);
            if (previousInstallationDate == null)
            {
                throw new BaseException("شناسه قبض یافت نشد");
            }

            //update
            await _InstallationDateAssignmentCommandService.Update(updateDto);
        }
    }
}
