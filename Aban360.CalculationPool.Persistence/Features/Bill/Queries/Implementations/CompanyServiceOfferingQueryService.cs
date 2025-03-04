using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Queries.Implementations
{
    public class CompanyServiceOfferingQueryService : ICompanyServiceOfferingQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<CompanyServiceOffering> _companyServiceOffering;
        public CompanyServiceOfferingQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _companyServiceOffering = _uow.Set<CompanyServiceOffering>();
            _companyServiceOffering.NotNull(nameof(_companyServiceOffering));
        }

        public async Task<CompanyServiceOffering> Get(short id)
        {
            return await _uow.FindOrThrowAsync<CompanyServiceOffering>(id);
        }

        public async Task<ICollection<CompanyServiceOffering>> Get()
        {
            return await _companyServiceOffering.ToListAsync();
        }
    }
}
