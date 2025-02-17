using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.DbSeeder.Implementations
{
    public class IndividualTypeSeeder : IDataSeeder
    {
        public int Order { get; set; } = 21;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<IndividualType> _IndividualType;
        public IndividualTypeSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _IndividualType = _uow.Set<IndividualType>();
            _IndividualType.NotNull(nameof(_IndividualType));
        }

        public void SeedData()
        {
            if (_IndividualType.Any())
            {
                return;
            }

            ICollection<IndividualType> IndividualType = new List<IndividualType>()
            {
                new IndividualType(){Id=1,Title="حقیقی"},
                new IndividualType(){Id=2,Title="حقوقی"}
            };
            _IndividualType.AddRange(IndividualType);
            _uow.SaveChanges();
        }
    }
}
