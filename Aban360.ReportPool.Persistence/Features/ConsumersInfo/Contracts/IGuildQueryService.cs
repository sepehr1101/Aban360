using Aban360.Common.BaseEntities;

namespace Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts
{
    public interface IGuildQueryService
    {
        Task<IEnumerable<NumericDictionary>> Get();
    }
}