using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts
{
    public interface IWaterModifiedBillsSummaryHandler
    {
        Task<ReportOutput<WaterModifiedBillsHeaderOutputDto, WaterModifiedBillsSummaryDataOutputDto>> Handle(WaterModifiedBillsInputDto input, CancellationToken cancellationToken);
    }
}
