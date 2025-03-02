using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Commands.Implementations
{
    public class InvoiceTypeCommandService : IInvoiceTypeCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<InvoiceType> _invoiceType;
        public InvoiceTypeCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _invoiceType = _uow.Set<InvoiceType>();
            _invoiceType.NotNull(nameof(InvoiceType));
        }

        public async Task Add(InvoiceType invoiceType)
        {
            await _invoiceType.AddAsync(invoiceType);
        }

        public async Task Remove(InvoiceType invoiceType)
        {
            _invoiceType.Remove(invoiceType);
        }
    }
}
