using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Queries.Implementations
{
    internal sealed class InvoiceInstallmentQueryService : IInvoiceInstallmentQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<InvoiceInstallment> _InvoinceInstallment;
        public InvoiceInstallmentQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _InvoinceInstallment = _uow.Set<InvoiceInstallment>();
            _InvoinceInstallment.NotNull(nameof(InvoiceInstallment));
        }

        public async Task<InvoiceInstallment> Get(long id)
        {
            return await _uow.FindOrThrowAsync<InvoiceInstallment>(id);
        }

        public async Task<InvoiceInstallment> Get(string paymentId)
        {
            return  await _InvoinceInstallment
                .Where(i => i.PaymentId == paymentId)
                .FirstOrDefaultAsync();
            //.SingleAsync();
        }

        public async Task<ICollection<InvoiceInstallment>> Get()
        {
            return await _InvoinceInstallment.ToListAsync();
        }
    }
}
