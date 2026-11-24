using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Repositories
{
    internal sealed class InvoiceLineItemInsertModeRepository : IInvoiceLineItemInsertModeCommandService, IInvoiceLineItemInsertModeQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<InvoiceLineItemInsertMode> _invoinceLineItemInsertMode;
        public InvoiceLineItemInsertModeRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));
            _invoinceLineItemInsertMode = _uow.Set<InvoiceLineItemInsertMode>();
            _invoinceLineItemInsertMode.NotNull(nameof(InvoiceLineItemInsertMode));
        }

        public async Task Add(InvoiceLineItemInsertMode invoinceLineItemInsertMode)
        {
            await _invoinceLineItemInsertMode.AddAsync(invoinceLineItemInsertMode);
        }

        public async Task Remove(InvoiceLineItemInsertMode invoinceLineItemInsertMode)
        {
            _invoinceLineItemInsertMode.Remove(invoinceLineItemInsertMode);
        }

        public async Task<InvoiceLineItemInsertMode> Get(InvoiceLineItemInsertModeEnum id)
        {
            return await _uow.FindOrThrowAsync<InvoiceLineItemInsertMode>(id);
        }

        public async Task<ICollection<InvoiceLineItemInsertMode>> Get()
        {
            return await _invoinceLineItemInsertMode.ToListAsync();
        }
    }
}
