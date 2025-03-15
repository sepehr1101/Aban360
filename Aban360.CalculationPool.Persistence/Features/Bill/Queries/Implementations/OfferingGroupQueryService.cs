using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Queries.Implementations
{
   internal sealed class OfferingGroupQueryService : IOfferingGroupQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<OfferingGroup> _OfferingGroup;
        public OfferingGroupQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _OfferingGroup = _uow.Set<OfferingGroup>();
            _OfferingGroup.NotNull(nameof(OfferingGroup));
        }

        public async Task<OfferingGroup> Get(short id)
        {
            return await _uow.FindOrThrowAsync<OfferingGroup>(id);
        }

        public async Task<ICollection<OfferingGroup>> Get()
        {
            return await _OfferingGroup.ToListAsync();
        }
    }
}
