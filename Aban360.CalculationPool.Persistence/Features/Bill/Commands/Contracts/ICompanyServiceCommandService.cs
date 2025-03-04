using Aban360.CalculationPool.Domain.Features.Bill.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts
{
    public interface ICompanyServiceCommandService
    {
        Task Add(CompanyService companyService);
        Task Remove(CompanyService companyService);

    }
}
