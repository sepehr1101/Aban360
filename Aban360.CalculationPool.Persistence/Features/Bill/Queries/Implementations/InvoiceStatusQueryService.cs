using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Queries.Implementations
{
    public class InvoiceStatusQueryService : IInvoiceStatusQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<InvoiceStatus> _invoiceStatus;
        public InvoiceStatusQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _invoiceStatus = _uow.Set<InvoiceStatus>();
            _invoiceStatus.NotNull(nameof(InvoiceStatus));
        }

        public async Task<InvoiceStatus> Get(short id)
        {
            return await _uow.FindOrThrowAsync<InvoiceStatus>(id);
        }

        public async Task<ICollection<InvoiceStatus>> Get()
        {
            return await _invoiceStatus.ToListAsync();
        }
    }
}
