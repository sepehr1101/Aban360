using Aban360.Common.BaseEntities;
using System.Threading.Tasks;

namespace Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts
{
    public interface IZoneAllQueryAddhoc
    {
        Task<ICollection<NumericDictionary>> Get(CancellationToken cancellationToken);
    }
}
