using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.DbSeeder.Implementations
{
    public class CapacityCalculationIndexSeeder : IDataSeeder
    {
        public int Order { get; set; } = 24;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<CapacityCalculationIndex> _capacityCalculationIndex;
        public CapacityCalculationIndexSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _capacityCalculationIndex = _uow.Set<CapacityCalculationIndex>();
            _capacityCalculationIndex.NotNull(nameof(_capacityCalculationIndex));
        }

        public void SeedData()
        {
            if (_capacityCalculationIndex.Any())
            {
                return;
            }

            ICollection<CapacityCalculationIndex> capacityCalculationIndex = new List<CapacityCalculationIndex>()
            {
                new CapacityCalculationIndex(){Title="Sample ",Description=" nothing "},
            };
            _capacityCalculationIndex.AddRange(capacityCalculationIndex);
            _uow.SaveChanges();
        }
    }
}
