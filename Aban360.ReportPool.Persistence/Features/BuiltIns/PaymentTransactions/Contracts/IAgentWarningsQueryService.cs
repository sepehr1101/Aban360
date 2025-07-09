using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts
{
    public interface IAgentWarningsQueryService
    {
        Task<ReportOutput<AgentWarningsHeaderOutputDto, AgentWarningsDataOutputDto>> GetInfo(AgentWarningsInputDto input);
    }
}
