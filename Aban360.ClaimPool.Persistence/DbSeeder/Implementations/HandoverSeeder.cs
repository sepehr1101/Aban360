using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.DbSeeder.Implementations
{
    public class HandoverSeeder : IDataSeeder
    {
        public int Order { get; set; } = 24;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Handover> _useState;
        public HandoverSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _useState = _uow.Set<Handover>();
            _useState.NotNull(nameof(_useState));
        }

        public void SeedData()
        {
            if (_useState.Any())
            {
                return;
            }

            ICollection<Handover> handover = new List<Handover>()
            {
                new Handover(){Id=10,Title="مشخص نشده)عادی)"},
                new Handover(){Id=1,Title="عادی"},
                new Handover(){Id=2,Title="مقطوعی قدیم"},
                new Handover(){Id=3,Title="خانه عالم"},
                new Handover(){Id=4,Title="ساخت و ساز"},
                new Handover(){Id=5,Title="کد 5"},
                new Handover(){Id=6,Title="بهزیستی"},
                new Handover(){Id=7,Title="کمیته امداد"},
            };
           _useState.AddRange(handover);
            _uow.SaveChanges();
        }
    }
}
