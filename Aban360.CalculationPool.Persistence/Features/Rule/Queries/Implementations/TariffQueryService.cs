using Aban360.CalculationPool.Domain.Features.Rule.Entties;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Features.Rule.Queries.Implementations
{
   internal sealed class TariffQueryService : ITariffQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Tariff> _tariff;
        public TariffQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _tariff = _uow.Set<Tariff>();
            _tariff.NotNull(nameof(_tariff));
        }

        public async Task<Tariff> Get(int id)
        {
            return await _tariff
                .AsNoTracking()
                .Include(tariff => tariff.Offering)
                .Include(tariff => tariff.LineItemType)
                .Where(tariff => tariff.Id == id)
                .SingleAsync();
        }

        public async Task<ICollection<Tariff>> Get()
        {
            return await _tariff
                .AsNoTracking()
                .Include(tariff => tariff.Offering)
                .Include(tariff => tariff.LineItemType)
                .ToListAsync();
        }

        public async Task<ICollection<Tariff>> GetByLineItemType(short id)
        {
            return await _tariff
                .AsNoTracking()
                .Include(tariff => tariff.Offering)
                .Include(tariff => tariff.LineItemType)
                .Where(t => t.LineItemTypeId == id)
                .ToListAsync();
        }

        public async Task<ICollection<Tariff>> GetByOfferingId(short id)
        {
            return await _tariff
               .AsNoTracking()
               .Include(tariff => tariff.Offering)
               .Include(tariff => tariff.LineItemType)
               .Where(t => t.OfferingId == id)
               .ToListAsync();
        }
    }
}
