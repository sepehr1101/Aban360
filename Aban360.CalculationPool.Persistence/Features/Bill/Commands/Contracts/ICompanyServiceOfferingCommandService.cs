using Aban360.CalculationPool.Domain.Features.Bill.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts
{
    public interface ICompanyServiceOfferingCommandService
    {
        Task Add(CompanyServiceOffering companyServiceOffering);
        Task Remove(CompanyServiceOffering companyServiceOffering);

    }
}
