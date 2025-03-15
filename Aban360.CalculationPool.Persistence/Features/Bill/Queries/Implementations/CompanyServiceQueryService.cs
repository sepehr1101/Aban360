using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Queries.Implementations
{
   internal sealed class CompanyServiceQueryService : ICompanyServiceQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<CompanyService> _companyService;
        public CompanyServiceQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _companyService = _uow.Set<CompanyService>();
            _companyService.NotNull(nameof(_companyService));
        }

        public async Task<CompanyService> Get(short id)
        {
            return await _companyService
                .Include(c => c.CompanyServiceType)
                .Where(c => c.Id == id)
                .SingleAsync();
        }

        public async Task<ICollection<CompanyService>> Get()
        {
            return await _companyService
                .Include(c => c.CompanyServiceType)
                .ToListAsync();
        }
    }
}
