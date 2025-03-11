using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.DbSeeder.Implementations
{
    public class InvoiceLineItemInsertModeSeeder : IDataSeeder
    {
        public int Order { get; set; } = 11;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<InvoiceLineItemInsertMode> _invoiceLineItemInsertModes;
        public InvoiceLineItemInsertModeSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _invoiceLineItemInsertModes = _uow.Set<InvoiceLineItemInsertMode>();
            _invoiceLineItemInsertModes.NotNull(nameof(_invoiceLineItemInsertModes));
        }

        public void SeedData()
        {
            if (_invoiceLineItemInsertModes.Any())
            {
                return;
            }

            var InvoiceLineItemInsertModes = GetInvoiceLineItemInsertModes();
            _invoiceLineItemInsertModes.AddRange(InvoiceLineItemInsertModes);
            _uow.SaveChanges();
        }
        private ICollection<InvoiceLineItemInsertMode> GetInvoiceLineItemInsertModes()
        {
            ICollection<InvoiceLineItemInsertMode> InvoiceLineItemInsertModes = new List<InvoiceLineItemInsertMode>()
            {
                new InvoiceLineItemInsertMode(){Id=InvoiceLineItemInsertModeEnum.ByUser,Title="توسط کاربر"},
                new InvoiceLineItemInsertMode(){Id=InvoiceLineItemInsertModeEnum.BySystem,Title="بصورت سیستمی"},
            };

            return InvoiceLineItemInsertModes;
        }
    }
}
