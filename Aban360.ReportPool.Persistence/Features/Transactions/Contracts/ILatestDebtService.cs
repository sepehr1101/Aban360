using Aban360.ReportPool.Domain.Features.Transactions;

namespace Aban360.ReportPool.Persistence.Features.Transactions.Contracts
{
    public interface ILatestDebtService
    {
        Task<LatestDebtDto> GetLatestDebt(string billId);
    }
}