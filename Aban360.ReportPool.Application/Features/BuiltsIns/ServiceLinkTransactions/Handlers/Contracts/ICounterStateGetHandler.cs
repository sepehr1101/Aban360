using Aban360.Common.BaseEntities;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts
{
    public interface ICounterStateGetHandler
    {
        Task<IEnumerable<StringDictionary>> Handle(CancellationToken cancellationToken);
    }
}
