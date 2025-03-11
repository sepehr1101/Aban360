using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Queries.Implementations
{
    public class OfferingQueryService : IOfferingQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Offering> _Offering;
        public OfferingQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _Offering = _uow.Set<Offering>();
            _Offering.NotNull(nameof(Offering));
        }

        public async Task<Offering> Get(short id)
        {
            return await _Offering
                .Include(o => o.OfferingGroup)
                .Include(o => o.OfferingUnit)
                .Where(o => o.Id == id)
                .SingleAsync();
        }

        public async Task<ICollection<Offering>> Get()
        {
            return await _Offering
                .Include(o => o.OfferingGroup)
                .Include(o => o.OfferingUnit)
                .ToListAsync();
        }
    }
}
