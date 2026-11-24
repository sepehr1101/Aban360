using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Repositories
{
    internal sealed class CompanyServiceTypeRepository : ICompanyServiceTypeCommandService, ICompanyServiceTypeQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<CompanyServiceType> _companyServiceType;
        public CompanyServiceTypeRepository(IUnitOfWork uow)
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
