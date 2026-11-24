using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Repositories
{
    internal sealed class InvoiceStatusRepository : IInvoiceStatusCommandService, IInvoiceStatusQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<InvoiceStatus> _invoiceStatus;
        public InvoiceStatusRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));
            _invoiceStatus = _uow.Set<InvoiceStatus>();
            _invoiceStatus.NotNull(nameof(InvoiceStatus));
        }

        public async Task Add(InvoiceStatus invoiceStatus)
        {
            await _invoiceStatus.AddAsync(invoiceStatus);
        }

        public async Task Remove(InvoiceStatus invoiceStatus)
        {
            _invoiceStatus.Remove(invoiceStatus);
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
