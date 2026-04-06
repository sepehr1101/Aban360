using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Domain.Features.Transactions;

namespace Aban360.ReportPool.Persistence.Features.Transactions.Contracts
{
    public interface IBranchEventSummaryWithLastDbQueryService
    {
        Task<ReportOutput<BranchEventSummaryHeaderOutputDto, BranchEventSummaryDataOutputDto>> Get(CardexInputDto input);
    }
}
