using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts
{
    public interface ISewageWaterInstallationSummaryByZoneIdGroupingHandler
    {
        Task<ReportOutput<SewageWaterInstallationHeaderOutputDto, ReportOutput<SewageWaterInstallationSummaryByZoneIdDateOutputDto, SewageWaterInstallationSummaryByZoneIdDateOutputDto>>> Handle(SewageWaterInstallationInputDto input, CancellationToken cancellationToken);
        Task<ReportOutput<SewageWaterInstallationHeaderOutputDto, SewageWaterInstallationSummaryByZoneIdDateOutputDto>> HandleFlat(SewageWaterInstallationInputDto input, CancellationToken cancellationToken);
    }
}
