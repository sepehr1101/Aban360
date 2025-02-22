namespace Aban360.ReportPool.Persistence.Queries.Implementations
{
    public interface IIndividualSummeryQueryService
    {
        Task<IndividualSummaryDto> GetOwnerShipSummery(string billId, short relationTypeId);
        Task<IndividualSummaryDto> GetStakeHolderSummery(string billId, short relationTypeId);
    }
}
