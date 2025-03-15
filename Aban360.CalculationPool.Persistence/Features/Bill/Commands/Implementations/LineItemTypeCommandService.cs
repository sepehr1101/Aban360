using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Commands.Implementations
{
   internal sealed class LineItemTypeCommandService : ILineItemTypeCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<LineItemType> _lineItemType;
        public LineItemTypeCommandService(IUnitOfWork uow)
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
    }
}
