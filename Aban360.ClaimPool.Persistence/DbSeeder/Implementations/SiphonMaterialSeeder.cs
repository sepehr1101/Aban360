using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.DbSeeder.Implementations
{
    public class SiphonMaterialSeeder : IDataSeeder
    {
        public int Order { get; set; } = 23;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<SiphonMaterial> _siphonMaterial;
        public SiphonMaterialSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _siphonMaterial = _uow.Set<SiphonMaterial>();
            _siphonMaterial.NotNull(nameof(_siphonMaterial));
        }

        public void SeedData()
        {
            if (_siphonMaterial.Any())
            {
                return;
            }

            ICollection<SiphonMaterial> SiphonMaterial = new List<SiphonMaterial>()
            {
                new SiphonMaterial(){Id=1,Title="پلی‌پروپیلن"},
                new SiphonMaterial(){Id=2,Title="نیکل"},
                new SiphonMaterial(){Id=3,Title="کروم"},
                new SiphonMaterial(){Id=4,Title="برنج"},
            };
            _siphonMaterial.AddRange(SiphonMaterial);
            _uow.SaveChanges();
        }
    }
}
