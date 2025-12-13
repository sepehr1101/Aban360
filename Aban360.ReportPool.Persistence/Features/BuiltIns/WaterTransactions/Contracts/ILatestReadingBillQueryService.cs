using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts
{
    public interface ILatestReadingBillQueryService
    {
        Task<LatestReadingBillDataOutputDto> GetInfo(string billId);
    }
}
