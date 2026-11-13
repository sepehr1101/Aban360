// Test-only contract for unchanged pre-refactor snapshots.
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Queries;

namespace Aban360.InstallationPool.Application.Features.Definition.Handlers.Queries.Contracts
{
    public interface IEquipmentBrokerGetSingleHandler
    {
        Task<EquipmentBrokerGetDto> Handle(short id, CancellationToken cancellationToken);
    }
}