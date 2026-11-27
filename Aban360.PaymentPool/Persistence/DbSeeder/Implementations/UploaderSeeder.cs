using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Constansts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;
using Aban360.PaymentPool.Domain.Features.Remuneration.Entities;
using Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.PaymentPool.Persistence.DbSeeder.Implementations
{
    public class UploaderSeeder : IDataSeeder
    {
        public int Order { get; set; } = 10;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Uploader> _uploader;
        public UploaderSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _uploader = _uow.Set<Uploader>();
            _uploader.NotNull(nameof(_uploader));
        }

        public void SeedData()
        {
            if (_uploader.Any())
            {
                return;
            }

            ICollection<Uploader> Uploaders = new List<Uploader>()
            {
                new Uploader()//sample for Convert InvoiceBranch
                {
                    Amount=1,
                    BankId=1,
                    InsertDateTime=DateTime.Now,
                    InsertRecordCount=0,
                    UserId=Guid.Empty,
                    Username="sample",
                }
            };

            _uploader.AddRange(Uploaders);
            _uow.SaveChanges();
        }
    }
}