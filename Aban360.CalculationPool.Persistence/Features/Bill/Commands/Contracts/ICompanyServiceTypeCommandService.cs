using Aban360.CalculationPool.Domain.Features.Bill.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts
{
    public interface ICompanyServiceTypeCommandService
    {
        Task Add(CompanyServiceType companyServiceType);
        Task Remove(CompanyServiceType companyServiceType);

    }
}
