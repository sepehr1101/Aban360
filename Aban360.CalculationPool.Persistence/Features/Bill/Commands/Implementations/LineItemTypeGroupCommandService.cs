using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Commands.Implementations
{
   internal sealed class LineItemTypeGroupCommandService : ILineItemTypeGroupCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<LineItemTypeGroup> _lineItemTypeGroup;
        public LineItemTypeGroupCommandService(IUnitOfWork uow)
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
    }
}
