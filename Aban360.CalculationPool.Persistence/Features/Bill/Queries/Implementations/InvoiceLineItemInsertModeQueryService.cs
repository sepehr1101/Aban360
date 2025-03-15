using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Queries.Implementations
{
   internal sealed class InvoiceLineItemInsertModeQueryService : IInvoiceLineItemInsertModeQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<InvoiceLineItemInsertMode> _InvoinceLineItemInsertMode;
        public InvoiceLineItemInsertModeQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _InvoinceLineItemInsertMode = _uow.Set<InvoiceLineItemInsertMode>();
            _InvoinceLineItemInsertMode.NotNull(nameof(InvoiceLineItemInsertMode));
        }

        public async Task<InvoiceLineItemInsertMode> Get(InvoiceLineItemInsertModeEnum id)
        {
            return await _uow.FindOrThrowAsync<InvoiceLineItemInsertMode>(id);
        }

        public async Task<ICollection<InvoiceLineItemInsertMode>> Get()
        {
            return await _InvoinceLineItemInsertMode.ToListAsync();
        }
    }
}
