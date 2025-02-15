using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.DbSeeder.Implementations
{
    public class UseStateSeeder : IDataSeeder
    {
        public int Order { get; set; } = 24;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<UseState> _useState;
        public UseStateSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _useState = _uow.Set<UseState>();
            _useState.NotNull(nameof(_useState));
        }

        public void SeedData()
        {
            if (_useState.Any())
            {
                return;
            }

            ICollection<UseState> UseState = new List<UseState>()
            {
                new UseState(){Id=1,Title="حذف موقت"},
                new UseState(){Id=2,Title="برقرار"},
                new UseState(){Id=3,Title="جمع آوری شده"},
            };
            _useState.AddRange(UseState);
            _uow.SaveChanges();
        }
    }
}
