using Aban360.ReportPool.Domain.Constants;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
public interface IWaterReceivablesQueryService
{
    Task<IEnumerable<WaterReceivablesAmountOutputDto>> GetSales(WaterReceivablesGroupingEnum groupBy, string fromDate, string toDate, CancellationToken cancellationToken);
    Task<IEnumerable<WaterReceivablesAmountOutputDto>> GetCollections(WaterReceivablesGroupingEnum groupBy, string fromDate, string toDate, CancellationToken cancellationToken);
}
