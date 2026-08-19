using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts
{
    public interface IFinancialStatementQueryService
    {
        Task<IEnumerable<FinancialStatementDataOutputDto>> GetWaterTotal(FinancialStatementInputDto input);
    }
}
