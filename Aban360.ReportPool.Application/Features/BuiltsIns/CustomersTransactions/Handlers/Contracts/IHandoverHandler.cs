using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts
{
    public interface IHandoverHandler
    {
        Task<IEnumerable<HandoverQueryDto>> Handle(CancellationToken cancellationToken);
    }
}
