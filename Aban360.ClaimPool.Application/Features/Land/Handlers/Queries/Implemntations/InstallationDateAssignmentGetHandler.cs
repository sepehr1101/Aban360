using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    internal sealed class InstallationDateAssignmentGetHandler : IInstallationDateAssignmentGetHandler
    {
        private readonly IInstallationDateAssignmentQueryService _InstallationDateAssignmentQueryService;
        public InstallationDateAssignmentGetHandler(IInstallationDateAssignmentQueryService InstallationDateAssignmentQueryService)
        {
            _InstallationDateAssignmentQueryService = InstallationDateAssignmentQueryService;
            _InstallationDateAssignmentQueryService.NotNull(nameof(InstallationDateAssignmentQueryService));
        }

        public async Task<InstallationDateAssignmentGetDto> Handle(string input, CancellationToken cancellationToken)
        {
            InstallationDateAssignmentGetDto installationDateAssignment = await _InstallationDateAssignmentQueryService.Get(input);
            return installationDateAssignment;
        }
    }
}
