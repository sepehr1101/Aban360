namespace Aban360.ReportPool.Persistence.Queries.Implementations
{
    public interface IConsumerSummaryInfo
    {
        Task<ResultSummaryDto> GetSummery(string id);
    }

}