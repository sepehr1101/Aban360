using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Commands.Implementations
{
    internal sealed class InvoiceLineItemCommandService : IInvoiceLineItemCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<InvoiceLineItem> _invoiceLineItem;
        public InvoiceLineItemCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _invoiceLineItem = _uow.Set<InvoiceLineItem>();
            _invoiceLineItem.NotNull(nameof(InvoiceLineItem));
        }

        public async Task Add(InvoiceLineItem invoiceLineItem)
        {
            await _invoiceLineItem.AddAsync(invoiceLineItem);
        }
        public async Task Add(ICollection<InvoiceLineItem> invoiceLineItem)
        {
            await _invoiceLineItem.AddRangeAsync(invoiceLineItem);
        }

        public async Task Remove(InvoiceLineItem invoiceLineItem)
        {
            _invoiceLineItem.Remove(invoiceLineItem);
        }
    }
}
