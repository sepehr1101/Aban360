using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.DbSeeder.Implementations
{
    public class ConstructionTypeSeeder : IDataSeeder
    {
        public int Order { get; set; } = 21;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<ConstructionType> _constructionType;
        public ConstructionTypeSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _constructionType = _uow.Set<ConstructionType>();
            _constructionType.NotNull(nameof(_constructionType));
        }

        public void SeedData()
        {
            if (_constructionType.Any())
            {
                return;
            }

            ICollection<ConstructionType> constructionType = new List<ConstructionType>()
            {
                new ConstructionType(){Id=1,Title="ویلایی"},
                new ConstructionType(){Id=2,Title="آپارتمان"},
            };
            _constructionType.AddRange(constructionType);
            _uow.SaveChanges();
        }
    }
}
