using Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts;

namespace Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts
{
    public interface IHeadquartersAddhoc
    {
        Task<string> Handle(short id, CancellationToken cancellationToken);
    }
}
