using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Repositories
{
    internal sealed class InvoiceInstallmentRepository : IInvoiceInstallmentCommandService, IInvoiceInstallmentQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<InvoiceInstallment> _invoiceInstallment;
        public InvoiceInstallmentRepository(IUnitOfWork uow)
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
        public async Task Add(ICollection<InvoiceInstallment> invoiceInstallments)
        {
            await _invoiceInstallment.AddRangeAsync(invoiceInstallments);
        }

        public async Task Remove(InvoiceInstallment invoiceInstallment)
        {
            _invoiceInstallment.Remove(invoiceInstallment);
        }

        public async Task<InvoiceInstallment> Get(long id)
        {
            return await _uow.FindOrThrowAsync<InvoiceInstallment>(id);
        }

        public async Task<InvoiceInstallment> Get(string paymentId)
        {
            return  await _invoiceInstallment
                .Where(i => i.PaymentId == paymentId)
                .FirstOrDefaultAsync();
            //.SingleAsync();
        }

        public async Task<ICollection<InvoiceInstallment>> Get()
        {
            return await _invoiceInstallment.ToListAsync();
        }
    }
}
