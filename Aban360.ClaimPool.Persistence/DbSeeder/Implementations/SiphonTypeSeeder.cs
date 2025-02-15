using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.DbSeeder.Implementations
{
public class SiphonTypeSeeder:IDataSeeder
    {
        public int Order { get; set; } = 24;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<SiphonType> _siphonType;
        public SiphonTypeSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _siphonType = _uow.Set<SiphonType>();
            _siphonType.NotNull(nameof(_siphonType));
        }

        public void SeedData()
        {
            if (_siphonType.Any())
            {
                return;
            }

            ICollection<SiphonType> siphonType = new List<SiphonType>()
            {
                new SiphonType(){Id=1,Title="شترگلو"},
                new SiphonType(){Id=2,Title="خم"},
                new SiphonType(){Id=3,Title="خشک"},
                new SiphonType(){Id=4,Title="روغنی"},
            };
            _siphonType.AddRange(siphonType);
            _uow.SaveChanges();
        }
    }
}
