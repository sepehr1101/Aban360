namespace Aban360.ReportPool.Persistence.Queries.Implementations
{
    public interface IEstateSummeryQueryService
    {
        Task<IEnumerable<ResultEstateDto>> GetSummery(string billId);
    }
}
