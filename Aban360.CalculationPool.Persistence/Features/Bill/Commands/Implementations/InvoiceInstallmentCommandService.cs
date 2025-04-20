using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Commands.Implementations
{
    internal sealed class InvoiceInstallmentCommandService : IInvoiceInstallmentCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<InvoiceInstallment> _invoiceInstallment;
        public InvoiceInstallmentCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _invoiceInstallment = _uow.Set<InvoiceInstallment>();
            _invoiceInstallment.NotNull(nameof(InvoiceInstallment));
        }

        public async Task Add(InvoiceInstallment invoiceInstallment)
        {
            await _invoiceInstallment.AddAsync(invoiceInstallment);
        }

        public async Task Remove(InvoiceInstallment invoiceInstallment)
        {
            _invoiceInstallment.Remove(invoiceInstallment);
        }
    }
}
