using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.DbSeeder.Implementations
{
    public class IndividualEstateRelationTypeSeeder : IDataSeeder
    {
        public int Order { get; set; } = 21;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<IndividualEstateRelationType> _individualEstateRelationType;
        public IndividualEstateRelationTypeSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _individualEstateRelationType = _uow.Set<IndividualEstateRelationType>();
            _individualEstateRelationType.NotNull(nameof(_individualEstateRelationType));
        }

        public void SeedData()
        {
            if (_individualEstateRelationType.Any())
            {
                return;
            }

            ICollection<IndividualEstateRelationType> IndividualEstateRelationType = new List<IndividualEstateRelationType>()
            {
                new IndividualEstateRelationType(){Id=IndividualEstateRelationTypeEnum.OwnerShip,Title="مالک"},
                new IndividualEstateRelationType(){Id=IndividualEstateRelationTypeEnum.Dwellty,Title="ساکن"},
                new IndividualEstateRelationType(){Id=IndividualEstateRelationTypeEnum.Tenant,Title="مستاجر"},
            };
            _individualEstateRelationType.AddRange(IndividualEstateRelationType);
            _uow.SaveChanges();
        }
    }
}
