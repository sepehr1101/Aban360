using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Commands.Implementations
{
   internal sealed class CompanyServiceTypeCommandService : ICompanyServiceTypeCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<CompanyServiceType> _companyServiceType;
        public CompanyServiceTypeCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _companyServiceType = _uow.Set<CompanyServiceType>();
            _companyServiceType.NotNull(nameof(_companyServiceType));
        }

        public async Task Add(CompanyServiceType companyServiceType)
        {
            await _companyServiceType.AddAsync(companyServiceType);
        }

        public async Task Remove(CompanyServiceType companyServiceType)
        {
            _companyServiceType.Remove(companyServiceType);
        }
    }
}
