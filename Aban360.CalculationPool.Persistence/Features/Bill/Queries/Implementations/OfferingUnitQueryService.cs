using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Queries.Implementations
{
    public class OfferingUnitQueryService : IOfferingUnitQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<OfferingUnit> _OfferingUnit;
        public OfferingUnitQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _OfferingUnit = _uow.Set<OfferingUnit>();
            _OfferingUnit.NotNull(nameof(OfferingUnit));
        }

        public async Task<OfferingUnit> Get(short id)
        {
            return await _uow.FindOrThrowAsync<OfferingUnit>(id);
        }

        public async Task<ICollection<OfferingUnit>> Get()
        {
            return await _OfferingUnit.ToListAsync();
        }
    }
}
