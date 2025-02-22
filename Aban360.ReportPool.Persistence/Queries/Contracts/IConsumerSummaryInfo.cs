namespace Aban360.ReportPool.Persistence.Queries.Implementations
{
    public interface IConsumerSummaryInfo
    {
        Task<ResultSummeryDto> GetSummery(string id);
    }

}