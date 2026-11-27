using Aban360.Common.BaseEntities;

namespace Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts
{
    public interface IProvienceQueryAddhoc
    {
        Task<ICollection<NumericDictionary>> Get(CancellationToken cancellationToken);
    }
}
