using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts
{
    public interface IAgentWarningsHandler
    {
        Task<ReportOutput<AgentWarningsHeaderOutputDto, AgentWarningsDataOutputDto>> Handle(AgentWarningsInputDto input, CancellationToken cancellationToken);
    }
}
