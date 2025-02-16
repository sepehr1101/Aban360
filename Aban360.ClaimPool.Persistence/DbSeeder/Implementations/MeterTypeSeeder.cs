using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.DbSeeder.Implementations
{
    public class MeterTypeSeeder : IDataSeeder
    {
        public int Order { get; set; } = 14;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<MeterType> _meterType;
        public MeterTypeSeeder(IUnitOfWork uow)
        {
            _uow=uow;
            _uow.NotNull(nameof(_uow));

            _meterType=_uow.Set<MeterType>();
            _meterType.NotNull(nameof(_meterType));
        }

        public void SeedData()
        {
            if (_meterType.Any())
            {
                return;
            }

            ICollection<MeterType> meterTypes = new List<MeterType>()
            {
                new MeterType(){Id=1,Title="هوشمند"},
                new MeterType(){Id=2,Title="معمولی"},
            };
            _meterType.AddRange(meterTypes);
            _uow.SaveChanges();
        }
    }
}
