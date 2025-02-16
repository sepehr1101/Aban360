using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.DbSeeder.Implementations
{
    public class MeterProducerSeeder : IDataSeeder
    {
        public int Order { get; set; } = 4;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<MeterProducer> _meterProducer;
        public MeterProducerSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _meterProducer=_uow.Set<MeterProducer>();
            _meterProducer.NotNull(nameof(_meterProducer));
        }

        public void SeedData()
        {
            if (_meterProducer.Any())
            {
                return;
            }

            ICollection<MeterProducer> meterProducer = new List<MeterProducer>()
            {
                new MeterProducer(){Id=1,Title="ایران انشعاب"},
                new MeterProducer(){Id=2,Title="مترآب"},
                new MeterProducer(){Id=3,Title="کسراسان"},
                new MeterProducer(){Id=4,Title="آب بان صنعتگران"},
                new MeterProducer(){Id=5,Title="اکباتان"},
            };
            _meterProducer.AddRange(meterProducer);
            _uow.SaveChanges();
        }
    }
}
