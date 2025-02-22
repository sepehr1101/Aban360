namespace Aban360.ReportPool.Persistence.Queries.Implementations
{
    public interface IEstateSummeryQueryService
    {
        Task<ResultEstateDto> GetSummery(string billId);
    }
}
