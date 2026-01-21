using Aban360.CalculationPool.Domain.Features.Bill.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts
{
    public interface ICompanyServiceQueryService
    {
        Task<CompanyService> Get(short id);
        Task<ICollection<CompanyService>> Get();
        Task<ICollection<CompanyService>> GetByTypeId(int typeId);
    }
}
