using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.DbSeeder.Implementations
{
    public class InvoiceTypeSeeder : IDataSeeder
    {
        public int Order { get; set; } = 11;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<InvoiceType> _invoiceType;
        public InvoiceTypeSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _invoiceType = _uow.Set<InvoiceType>();
            _invoiceType.NotNull(nameof(_invoiceType));
        }

        public void SeedData()
        {
            if (_invoiceType.Any())
            {
                return;
            }

            ICollection<InvoiceType> invoiceType = new List<InvoiceType>()
            {
                new InvoiceType(){Id=1,Title="آب بها",Description=""},
                new InvoiceType(){Id=2,Title="انشعاب ",Description=""},
            };
            _invoiceType.AddRange(invoiceType);
            _uow.SaveChanges();
        }
    } 
}
