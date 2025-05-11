using Aban360.Common.BaseEntities;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Contracts
{
    public interface ITariffConstantDictionaryHandler
    {
        Task<ICollection<StringDictionary>> Handle(CancellationToken cancellationToken);
    }
}
