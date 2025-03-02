using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Queries.Implementations
{
    public class InvoiceTypeQueryService : IInvoiceTypeQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<InvoiceType> _InvoiceType;
        public InvoiceTypeQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _InvoiceType = _uow.Set<InvoiceType>();
            _InvoiceType.NotNull(nameof(InvoiceType));
        }

        public async Task<InvoiceType> Get(short id)
        {
            return await _uow.FindOrThrowAsync<InvoiceType>(id);
        }

        public async Task<ICollection<InvoiceType>> Get()
        {
            return await _InvoiceType.ToListAsync();
        }
    }
}
