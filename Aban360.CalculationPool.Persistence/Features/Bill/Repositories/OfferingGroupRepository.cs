using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Repositories
{
    internal sealed class OfferingGroupRepository : IOfferingGroupCommandService, IOfferingGroupQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<OfferingGroup> _offeringGroup;
        public OfferingGroupRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));
            _offeringGroup = _uow.Set<OfferingGroup>();
            _offeringGroup.NotNull(nameof(OfferingGroup));
        }

        public async Task Add(OfferingGroup offeringGroup)
        {
            await _offeringGroup.AddAsync(offeringGroup);
        }

        public async Task Remove(OfferingGroup offeringGroup)
        {
            _offeringGroup.Remove(offeringGroup);
        }

        public async Task<OfferingGroup> Get(short id)
        {
            return await _uow.FindOrThrowAsync<OfferingGroup>(id);
        }

        public async Task<ICollection<OfferingGroup>> Get()
        {
            return await _offeringGroup.ToListAsync();
        }
    }
}
