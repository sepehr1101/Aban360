using Aban360.CalculationPool.Domain.Features.Bill.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts
{
    public interface ICompanyServiceOfferingQueryService
    {
        Task<CompanyServiceOffering> Get(short id);
        Task<ICollection<CompanyServiceOffering>> Get();
    }
}
