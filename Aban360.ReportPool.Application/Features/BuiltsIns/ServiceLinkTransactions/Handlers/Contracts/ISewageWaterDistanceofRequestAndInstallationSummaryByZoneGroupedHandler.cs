using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts
{
    public interface ISewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedHandler
    {
        Task<ReportOutput<SewageWaterDistanceofRequestAndInstallationHeaderOutputDto, ReportOutput<SewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedDataOutputDto, SewageWaterDistanceofRequestAndInstallationSummaryByZoneGroupedDataOutputDto>>> Handle(SewageWaterDistanceofRequestAndInstallationByZoneInputDto input, CancellationToken cancellationToken);
    }
}
