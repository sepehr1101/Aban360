using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.Common.BaseEntities;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts
{
    public interface ICompanyServiceQueryService
    {
        Task<CompanyService> Get(short id);
        Task<ICollection<CompanyService>> Get();
        Task<ICollection<NumericDictionary>> GetByTypeId(int typeId);
    }
}
