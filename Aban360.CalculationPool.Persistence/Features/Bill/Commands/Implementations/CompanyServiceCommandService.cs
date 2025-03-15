using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Commands.Implementations
{
   internal sealed class CompanyServiceCommandService : ICompanyServiceCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<CompanyService> _companyService;
        public CompanyServiceCommandService(IUnitOfWork uow)
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
    }
}
