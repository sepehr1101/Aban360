using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Repositories
{
    internal sealed class CompanyServiceOfferingRepository : ICompanyServiceOfferingCommandService, ICompanyServiceOfferingQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<CompanyServiceOffering> _companyServiceOffering;
        public CompanyServiceOfferingRepository(IUnitOfWork uow)
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

        public async Task<CompanyServiceOffering> Get(short id)
        {
            return await _companyServiceOffering
                .Include(cso => cso.CompanyService)
                .Include(cso => cso.Offering)
                .SingleAsync();
        }

        public async Task<ICollection<CompanyServiceOffering>> Get()
        {
            return await _companyServiceOffering
                .Include(cso => cso.CompanyService)
                .Include(cso=>cso.Offering)
                .ToListAsync();
        }
    }
}
