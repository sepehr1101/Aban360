namespace Aban360.ReportPool.Persistence.Queries.Implementations
{
    public interface IWaterMeterSummeryQueryService
    {
        Task<WaterMeterSummaryDto> GetSummery(string billId, short meterDiameterId);
    }
}
