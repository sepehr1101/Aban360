using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts
{
    public interface IT46QueryService
    {
        Task<IEnumerable<NumericDictionary>> Get();
        Task<NumericDictionary> GetByZone(int zoneId, bool hasException);
    }
}
