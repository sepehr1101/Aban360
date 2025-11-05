using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts
{
    public interface IWaterDistanceDeliverToInstallSummaryByZoneGroupedHandler
    {
        Task<ReportOutput<SewageWaterDistanceHeaderOutputDto, ReportOutput<SewageWaterDistanceSummaryByZoneGroupedDataOutputDto, SewageWaterDistanceSummaryByZoneGroupedDataOutputDto>>> Handle(WaterDistanceDeliverToInstallInputDto input, CancellationToken cancellationToken);
        Task<ReportOutput<SewageWaterDistanceHeaderOutputDto, SewageWaterDistanceSummaryByZoneGroupedDataOutputDto>> HandleFlat(WaterDistanceDeliverToInstallInputDto input, CancellationToken cancellationToken);
    }
}
