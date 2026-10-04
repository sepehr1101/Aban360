using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts
{
    public interface ISewageWaterRequestSummaryQueryService
    {
        Task<ReportOutput<SewageWaterRequestSummaryHeaderOutputDto, SewageWaterRequestSummaryDataOutputDto>> Get(SewageWaterRequestInputDto input);
    }
}
