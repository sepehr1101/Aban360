using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.DbSeeder.Implementations
{
    public class EstateBoundTypeSeeder : IDataSeeder
    {
        public int Order { get; set; } = 22;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<EstateBoundType> _estateBoundTypes;
        public EstateBoundTypeSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _estateBoundTypes = _uow.Set<EstateBoundType>();
            _estateBoundTypes.NotNull(nameof(_estateBoundTypes));
        }

        public void SeedData()
        {
            if (_estateBoundTypes.Any())
            {
                return;
            }
            ICollection<EstateBoundType> estateBoundTypes = new List<EstateBoundType>()
            {
                new EstateBoundType(){Id=1,Title="داخل محدوده"},
                new EstateBoundType(){Id=2,Title="خارج محدوده" },
                new EstateBoundType(){Id=3,Title="طرح هادی" }
            };
            _estateBoundTypes.AddRange(estateBoundTypes);
            _uow.SaveChanges();
        }
    }
}