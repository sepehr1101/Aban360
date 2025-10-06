using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts
{
    public interface ISewageWaterRequestSummaryByZoneGroupingHandler
    {
        Task<ReportOutput<SewageWaterRequestHeaderOutputDto, ReportOutput<SewageWaterRequestSummaryByZoneIdGroupingDataOutputDto, SewageWaterRequestSummaryByZoneIdGroupingDataOutputDto>>> Handle(SewageWaterRequestInputDto input, CancellationToken cancellationToken);
        Task<ReportOutput<SewageWaterRequestHeaderOutputDto, SewageWaterRequestSummaryByZoneIdGroupingDataOutputDto>> HandleFlat(SewageWaterRequestInputDto input, CancellationToken cancellationToken);
    }
}
