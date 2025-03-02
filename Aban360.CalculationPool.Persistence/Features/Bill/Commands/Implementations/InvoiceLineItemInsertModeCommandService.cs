using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Commands.Implementations
{
    public class InvoiceLineItemInsertModeCommandService : IInvoiceLineItemInsertModeCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<InvoiceLineItemInsertMode> _invoinceLineItemInsertMode;
        public InvoiceLineItemInsertModeCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _invoinceLineItemInsertMode = _uow.Set<InvoiceLineItemInsertMode>();
            _invoinceLineItemInsertMode.NotNull(nameof(InvoiceLineItemInsertMode));
        }

        public async Task Add(InvoiceLineItemInsertMode invoinceLineItemInsertMode)
        {
            await _invoinceLineItemInsertMode.AddAsync(invoinceLineItemInsertMode);
        }

        public async Task Remove(InvoiceLineItemInsertMode invoinceLineItemInsertMode)
        {
            _invoinceLineItemInsertMode.Remove(invoinceLineItemInsertMode);
        }
    }
}
