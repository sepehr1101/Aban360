namespace Aban360.ReportPool.Persistence.Queries.Implementations
{
    public interface ISiphonSummeryQueryService
    {
        Task<IEnumerable<SiphonSummaryDto>> GetInfo(string billId);
    }
}
