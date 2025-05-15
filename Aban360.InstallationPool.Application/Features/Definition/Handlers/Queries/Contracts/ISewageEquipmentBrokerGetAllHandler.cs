using Aban360.InstallationPool.Domain.Features.Definition.Dto.Queries;

namespace Aban360.InstallationPool.Application.Features.Definition.Handlers.Queries.Contracts
{
    public interface ISewageEquipmentBrokerGetAllHandler
    {
        Task<ICollection<SewageEquipmentBrokerGetDto>> Handle(CancellationToken cancellationToken);
    }
}
