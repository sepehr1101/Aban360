using Aban360.CalculationPool.Domain.Features.Rule.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts
{
    public interface ITariffConstantQueryService
    {
        Task<TariffConstant> Get(short id);
        Task<ICollection<TariffConstant>> Get();
        Task<ICollection<TariffConstant>> Get(string @from, string @to);
    }
}
