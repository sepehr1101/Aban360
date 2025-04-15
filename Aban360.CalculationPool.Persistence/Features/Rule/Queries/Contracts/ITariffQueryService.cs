using Aban360.CalculationPool.Domain.Features.Rule.Entties;

namespace Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts
{
    public interface ITariffQueryService
    {
        Task<Tariff> Get(int id);
        Task<ICollection<Tariff>> Get();
        Task<ICollection<Tariff>> Get(string @from, string @to);
        Task<ICollection<Tariff>> GetByOfferingId(short id);
        Task<ICollection<Tariff>> GetByLineItemType(short id);
    }
}
