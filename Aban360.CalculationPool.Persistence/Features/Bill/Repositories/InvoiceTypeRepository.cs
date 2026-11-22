using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Repositories
{
    internal sealed class InvoiceTypeRepository : IInvoiceTypeCommandService, IInvoiceTypeQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<InvoiceType> _invoiceType;
        public InvoiceTypeRepository(IUnitOfWork uow)
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

        public async Task<InvoiceType> Get(short id)
        {
            return await _uow.FindOrThrowAsync<InvoiceType>(id);
        }

        public async Task<ICollection<InvoiceType>> Get()
        {
            return await _invoiceType.ToListAsync();
        }
    }
}
