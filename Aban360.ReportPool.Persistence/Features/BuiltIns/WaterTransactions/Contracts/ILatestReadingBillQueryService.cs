using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Domain.Features.Transactions;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts
{
    public interface ILatestReadingBillQueryService
    {
        Task<LatestReadingBillDataOutputDto> GetInfo(string billId);
        Task<LatestReadingBillDataOutputDto> GetInfo(ZoneIdAndCustomerNumberOutputDto input);
    }
}
