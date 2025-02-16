using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.DbSeeder.Implementations
{
    public class MeterMaterialSeeder : IDataSeeder
    {
        public int Order { get; set; } = 16;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<MeterMaterial> _meterMaterial;
        public MeterMaterialSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _meterMaterial = _uow.Set<MeterMaterial>();
            _meterMaterial.NotNull(nameof(_meterMaterial));
        }

        public void SeedData()
        {
            if (_meterMaterial.Any())
            {
                return;
            }

            ICollection<MeterMaterial> meterMaterial = new List<MeterMaterial>()
            {
                new MeterMaterial(){Id=1,Title="چدن خاکستری"},
                new MeterMaterial(){Id=2,Title="چدن داکتیل"},
                new MeterMaterial(){Id=3,Title="برنج"},
                new MeterMaterial(){Id=4,Title="پلاستیک مهندسی"},
                new MeterMaterial(){Id=5,Title="آلیاژ آلومینیوم"},
                new MeterMaterial(){Id=6,Title="فولاد ضد زنگ"},
            };
            _meterMaterial.AddRange(meterMaterial);
            _uow.SaveChanges();
        }
    }
}
