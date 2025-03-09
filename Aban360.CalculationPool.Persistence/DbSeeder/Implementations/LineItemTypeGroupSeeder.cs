using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.DbSeeder.Implementations
{
    public class LineItemTypeGroupSeeder : IDataSeeder
    {
        public int Order { get; set; } = 10;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<LineItemTypeGroup> _lineItemTypeGroups;
        public LineItemTypeGroupSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _lineItemTypeGroups = _uow.Set<LineItemTypeGroup>();
            _lineItemTypeGroups.NotNull(nameof(_lineItemTypeGroups));
        }

        public void SeedData()
        {
            if (_lineItemTypeGroups.Any())
            {
                return;
            }

            var lineItemTypeGroups = GetLineItemTypeGroups();
            _lineItemTypeGroups.AddRange(lineItemTypeGroups);
            _uow.SaveChanges();
        }
        private ICollection<LineItemTypeGroup> GetLineItemTypeGroups()
        {
            ICollection<LineItemTypeGroup> lineItemTypeGroups = new List<LineItemTypeGroup>()
            {
                new LineItemTypeGroup(){Id=1,Title="اصلی",ImpactSign=+1,Description=""},
                new LineItemTypeGroup(){Id=2,Title="مالیات",ImpactSign=+1,Description=""},
                new LineItemTypeGroup(){Id=3,Title="کارمزد",ImpactSign=+1,Description=""},
                new LineItemTypeGroup(){Id=4,Title="تخفیف",ImpactSign=-1,Description=""},
            };

            return lineItemTypeGroups;
        }
    }
}
