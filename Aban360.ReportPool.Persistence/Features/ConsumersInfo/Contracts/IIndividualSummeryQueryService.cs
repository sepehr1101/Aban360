using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;

namespace Aban360.ReportPool.Persistence.Queries.Implementations
{
    public interface IIndividualSummeryQueryService
    {
        Task<IEnumerable<IndividualSummaryDto>> GetOwnerShipSummery(string billId, short relationTypeId);
        Task<IEnumerable<IndividualSummaryDto>> GetStakeHolderSummery(string billId, short relationTypeId);
    }
}
