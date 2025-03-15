using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts
{
    public interface IChangeMeterReasonQueryService
    {
        Task<ChangeMeterReason> Get(ChangeMeterReasonEnum id);
        Task<ICollection<ChangeMeterReason>> Get();
    }
}
