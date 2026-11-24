using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.BaseEntities;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Repositories
{
    internal sealed class CompanyServiceRepository : ICompanyServiceCommandService, ICompanyServiceQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<CompanyService> _companyService;
        public CompanyServiceRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));
            _companyService = _uow.Set<CompanyService>();
            _companyService.NotNull(nameof(_companyService));
        }

        public async Task Add(CompanyService companyService)
        {
            await _companyService.AddAsync(companyService);
        }

        public async Task Remove(CompanyService companyService)
        {
            _companyService.Remove(companyService);
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
        public async Task<ICollection<NumericDictionary>> GetByTypeId(int typeId)
        {
            return await _companyService
                .Include(c => c.CompanyServiceType)
                .Where(c => c.CompanyServiceTypeId == typeId)
                .Select(c => new NumericDictionary(c.Id, c.Title))
                .ToListAsync();
        }
    }
}
