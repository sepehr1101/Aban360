using Aban360.Common.BaseEntities;

namespace Aban360.LocationPool.GatewayAdhoc.Features.MainHeirarchy.Contracts
{
    public interface IProvienceQueryAddhoc
    {
        Task<ICollection<NumericDictionary>> Get(CancellationToken cancellationToken);
    }
}
