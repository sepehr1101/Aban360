using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts
{
    public interface ISewageWaterDistanceofRequestAndInstallationSummaryQueryService
    {
        Task<ReportOutput<SewageWaterDistanceHeaderOutputDto, SewageWaterDistanceSummaryDataOutputDto>> Get(SewageWaterDistanceofRequestAndInstallationInputDto input);
    }
}
