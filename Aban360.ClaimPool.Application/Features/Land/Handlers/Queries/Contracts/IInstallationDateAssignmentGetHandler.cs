using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts
{
    public interface IInstallationDateAssignmentGetHandler
    {
        Task<InstallationDateAssignmentGetDto> Handle(string input, CancellationToken cancellationToken);
    }
}
