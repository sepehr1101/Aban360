using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts
{
    public interface IT52QueryService
    {
        Task<string> Get(ZoneIdAndCustomerNumber input);
    }
}
