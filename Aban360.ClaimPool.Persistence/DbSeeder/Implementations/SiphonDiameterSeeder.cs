using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.DbSeeder.Implementations
{
    public class SiphonDiameterSeeder : IDataSeeder
    {
        public int Order { get; set; } = 7;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<SiphonDiameter> _siphonDiameter;
        public SiphonDiameterSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _siphonDiameter = _uow.Set<SiphonDiameter>();
            _siphonDiameter.NotNull(nameof(_siphonDiameter));
        }

        public void SeedData()
        {
            if (_siphonDiameter.Any())
            {
                return;
            }

            ICollection<SiphonDiameter> siphonDiameters = new List<SiphonDiameter>()
            {
                new SiphonDiameter(){Id=1,Title="قطر 100"},
                new SiphonDiameter(){Id=2,Title="قطر 125"},
                new SiphonDiameter(){Id=3,Title="قطر 150"},
                new SiphonDiameter(){Id=4,Title="قطر 200"},
                new SiphonDiameter(){Id=5,Title="قطر 5"},
                new SiphonDiameter(){Id=6,Title="قطر 6"},
                new SiphonDiameter(){Id=7,Title="قطر 7"},
                new SiphonDiameter(){Id=8,Title="قطر 8"},
            };
            _siphonDiameter.AddRange(siphonDiameters);
            _uow.SaveChanges();
        }
    }

}
