namespace Aban360.ReportPool.Persistence.Queries.Implementations
{
    public interface IFlatSummeryQueryService
    {
        Task<ResultFlatDto> GetInfo(string billId);
    }
}
