using Aban360.Common.BaseEntities;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts
{
    public interface ICounterStateQueryService
    {
        Task<IEnumerable<StringDictionary>> Get();
    }
}
