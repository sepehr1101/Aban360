using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts
{
    public interface IRequestStatusQueryService
    {
        Task<IEnumerable<SelectionDto>> GetIsKartable();
    }
}
