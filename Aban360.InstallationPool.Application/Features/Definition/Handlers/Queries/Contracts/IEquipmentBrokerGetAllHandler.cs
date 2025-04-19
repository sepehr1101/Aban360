using Aban360.InstallationPool.Domain.Features.Definition.Dto.Queries;

namespace Aban360.InstallationPool.Application.Features.Definition.Handlers.Queries.Contracts
{
    public interface IEquipmentBrokerGetAllHandler
    {
        Task<ICollection<EquipmentBrokerGetDto>> Handle(CancellationToken cancellationToken);
    }
}
