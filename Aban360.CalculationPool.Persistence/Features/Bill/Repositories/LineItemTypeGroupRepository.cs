using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Repositories
{
    internal sealed class LineItemTypeGroupRepository : ILineItemTypeGroupCommandService, ILineItemTypeGroupQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<LineItemTypeGroup> _lineItemTypeGroup;
        public LineItemTypeGroupRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));
            _lineItemTypeGroup = _uow.Set<LineItemTypeGroup>();
            _lineItemTypeGroup.NotNull(nameof(_lineItemTypeGroup));
        }

        public async Task Add(LineItemTypeGroup lineItemTypeGroup)
        {
            await _lineItemTypeGroup.AddAsync(lineItemTypeGroup);
        }

        public async Task Remove(LineItemTypeGroup lineItemTypeGroup)
        {
            _lineItemTypeGroup.Remove(lineItemTypeGroup);
        }

        public async Task<LineItemTypeGroup> Get(short id)
        {
            return await _lineItemTypeGroup
                .Include(l => l.ImpactSign)
                .Where(l => l.Id == id)
                .SingleAsync();
        }

        public async Task<ICollection<LineItemTypeGroup>> Get()
        {
            return await _lineItemTypeGroup
                .Include(l => l.ImpactSign)
                .ToListAsync();
        }
    }
}
