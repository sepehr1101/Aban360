using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Queries.Implementations
{
   internal sealed class CompanyServiceTypeQueryService : ICompanyServiceTypeQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<CompanyServiceType> _companyServiceType;
        public CompanyServiceTypeQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _companyServiceType = _uow.Set<CompanyServiceType>();
            _companyServiceType.NotNull(nameof(_companyServiceType));
        }

        public async Task<CompanyServiceType> Get(short id)
        {
            return await _companyServiceType
                .Include(c => c.TariffCalculationMode)
                .Where(c => c.Id == id)
                .SingleAsync();
        }

        public async Task<ICollection<CompanyServiceType>> Get()
        {
            return await _companyServiceType
                .Include(c => c.TariffCalculationMode)
                .ToListAsync();
        }
    }
}
