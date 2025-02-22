namespace Aban360.ReportPool.Persistence.Queries.Implementations
{
    public interface ISiphonSummeryQueryService
    {
        Task<SiphonSummaryDto> GetSummery(string billId);
    }
}
