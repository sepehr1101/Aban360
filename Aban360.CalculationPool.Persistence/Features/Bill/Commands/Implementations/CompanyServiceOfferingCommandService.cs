using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Commands.Implementations
{
    public class CompanyServiceOfferingCommandService : ICompanyServiceOfferingCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<CompanyServiceOffering> _companyServiceOffering;
        public CompanyServiceOfferingCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _companyServiceOffering = _uow.Set<CompanyServiceOffering>();
            _companyServiceOffering.NotNull(nameof(_companyServiceOffering));
        }

        public async Task Add(CompanyServiceOffering companyServiceOffering)
        {
            await _companyServiceOffering.AddAsync(companyServiceOffering);
        }

        public async Task Remove(CompanyServiceOffering companyServiceOffering)
        {
            _companyServiceOffering.Remove(companyServiceOffering);
        }
    }
}
