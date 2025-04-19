using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.DbSeeder.Implementations
{
    public class InvoiceStatusSeeder : IDataSeeder
    {
        public int Order { get; set; } = 11;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<InvoiceStatus> _invoiceStatus;
        public InvoiceStatusSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _invoiceStatus = _uow.Set<InvoiceStatus>();
            _invoiceStatus.NotNull(nameof(_invoiceStatus));
        }

        public void SeedData()
        {
            if (_invoiceStatus.Any())
            {
                return;
            }

            ICollection<InvoiceStatus> invoiceStatus = new List<InvoiceStatus>()
            {
                new InvoiceStatus(){Id=1,Title="تایید شده",Description=""},
                new InvoiceStatus(){Id=2,Title="صادر شده",Description=""},
            };
            _invoiceStatus.AddRange(invoiceStatus);
            _uow.SaveChanges();
        }
    }
}
