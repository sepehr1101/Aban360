using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Implementations
{
    internal sealed class CounterStateGetHandler : ICounterStateGetHandler
    {
        private readonly ICounterStateQueryService _counterStateQuery;
        public CounterStateGetHandler(ICounterStateQueryService counterStateQuery)
        {
            _counterStateQuery = counterStateQuery;
            _counterStateQuery.NotNull(nameof(counterStateQuery));
        }

        public async Task<IEnumerable<StringDictionary>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<StringDictionary> result = await _counterStateQuery.Get();
            return result;
        }
    }
}
