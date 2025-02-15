using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.DbSeeder.Implementations
{
    public class MeterUseTypeSeeder : IDataSeeder
    {
        public int Order { get; set; } = 12;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<MeterUseType> _meterUseType;
        public MeterUseTypeSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _meterUseType = _uow.Set<MeterUseType>();
            _meterUseType.NotNull(nameof(_meterUseType));
        }

        public void SeedData()
        {
            if (_meterUseType.Any())
            {
                return;
            }

            ICollection<MeterUseType> meterUseType = new List<MeterUseType>()
            {
                new MeterUseType(){Id=1,Title="مصرف"},
                new MeterUseType(){Id=2,Title="شاهد"},
            };
            _meterUseType.AddRange(meterUseType);
            _uow.SaveChanges();
        }
    }
}
