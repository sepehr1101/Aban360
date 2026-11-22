using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Repositories
{
    internal sealed class LineItemTypeRepository : ILineItemTypeCommandService, ILineItemTypeQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<LineItemType> _lineItemType;
        public LineItemTypeRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));
            _lineItemType = _uow.Set<LineItemType>();
            _lineItemType.NotNull(nameof(_lineItemType));
        }

        public async Task Add(LineItemType lineItemType)
        {
            await _lineItemType.AddAsync(lineItemType);
        }

        public async Task Remove(LineItemType lineItemType)
        {
            _lineItemType.Remove(lineItemType);
        }

        public async Task<LineItemType> Get(short id)
        {
            return await _lineItemType
                .Include(l => l.LineItemTypeGroup)
                .Where(l => l.Id == id)
                .SingleAsync();
        }

        public async Task<ICollection<LineItemType>> Get()
        {
            return await _lineItemType
                .Include(l => l.LineItemTypeGroup)
                .ToListAsync();
        }
    }
}
