using Aban360.CalculationPool.Domain.Features.Bill.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts
{
    public interface ICompanyServiceTypeQueryService
    {
        Task<CompanyServiceType> Get(short id);
        Task<ICollection<CompanyServiceType>> Get();
    }
}
