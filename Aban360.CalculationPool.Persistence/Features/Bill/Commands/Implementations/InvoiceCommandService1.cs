using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Commands.Implementations
{
    internal sealed class InvoiceCommandService : IInvoiceCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Invoice> _invoice;
        public InvoiceCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _invoice = _uow.Set<Invoice>();
            _invoice.NotNull(nameof(Invoice));
        }

        public async Task Add(Invoice invoice)
        {
            await _invoice.AddAsync(invoice);
        }

        public async Task Remove(Invoice invoice)
        {
            _invoice.Remove(invoice);
        }
    }
}
