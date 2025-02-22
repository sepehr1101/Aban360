namespace Aban360.ReportPool.Persistence.Queries.Implementations
{
    public interface IWaterMeterSummeryQueryService
    {
        Task<WaterMeterSummaryDto> GetInfo(string billId, short meterDiameterId);
    }
}
