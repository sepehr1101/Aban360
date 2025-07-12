using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts
{
    public interface IWaterRawSalesSummaryHandler
    {
        Task<ReportOutput<WaterSalesHeaderOutputDto, WaterRawSalesSummaryDataOutputDto>> Handle(WaterSalesInputDto input, CancellationToken cancellationToken);
    }
}
