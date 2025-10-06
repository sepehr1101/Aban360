using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts
{
    public interface ISewageWaterRequestNonInstalledSummaryByZoneGroupedHandler
    {
        Task<ReportOutput<SewageWaterRequestNonInstalledHeaderOutputDto, ReportOutput<SewageWaterRequestNonInstalledSummaryDataOutputDto, SewageWaterRequestNonInstalledSummaryDataOutputDto>>> Handle(SewageWaterRequestNonInstalledInputDto input, CancellationToken cancellationToken);

        Task<ReportOutput<SewageWaterRequestNonInstalledHeaderOutputDto, SewageWaterRequestNonInstalledSummaryDataOutputDto>> HandleFlat(SewageWaterRequestNonInstalledInputDto input, CancellationToken cancellationToken);
    }
}
