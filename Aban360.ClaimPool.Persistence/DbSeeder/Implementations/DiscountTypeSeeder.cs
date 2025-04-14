using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.DbSeeder.Implementations
{
    public class DiscountTypeSeeder : IDataSeeder
    {
        public int Order { get; set; } = 30;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<DiscountType> _discountType;
        public DiscountTypeSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _discountType=_uow.Set<DiscountType>();
            _discountType.NotNull(nameof(_discountType));
        }

        public void SeedData()
        {
            if (_discountType.Any())
            {
                return;
            }

            ICollection<DiscountType> discountType = new List<DiscountType>()
            {
                new DiscountType(){Id=DiscountTypeEnum.Janbazan,Title="جانبازان"},
                new DiscountType(){Id=DiscountTypeEnum.KhanevadeShohada,Title="خانواده شهدا"},
            };
            _discountType.AddRange(discountType);
            _uow.SaveChanges();
        }
    }
}
