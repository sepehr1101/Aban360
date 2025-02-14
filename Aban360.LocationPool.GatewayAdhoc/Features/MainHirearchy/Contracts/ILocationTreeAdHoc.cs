using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries.ValueKeys;

namespace Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts
{
    public interface ILocationTreeAdHoc
    {
        Task<LocationTree> Handle(ICollection<int> selectedZoneIds, CancellationToken cancellationToken);
    }
}