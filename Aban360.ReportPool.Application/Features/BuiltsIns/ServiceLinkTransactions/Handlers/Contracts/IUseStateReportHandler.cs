using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts
{
    public interface IUseStateReportHandler
    {
        Task<ICollection<UseStateReportOutputDto>> Handle(UseStateReportInputDto input,CancellationToken cancellationToken);
    }
}
