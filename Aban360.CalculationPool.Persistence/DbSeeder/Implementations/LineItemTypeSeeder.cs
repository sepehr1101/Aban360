using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.DbSeeder.Implementations
{
    public class LineItemTypeSeeder : IDataSeeder
    {
        public int Order { get; set; } = 10;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<LineItemType> _lineItemType;
        public LineItemTypeSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _lineItemType = _uow.Set<LineItemType>();
            _lineItemType.NotNull(nameof(_lineItemType));
        }

        public void SeedData()
        {
            if (_lineItemType.Any())
            {
                return;
            }

            var lineItemTypes = GetLineItemType();
            _lineItemType.AddRange(lineItemTypes);
            _uow.SaveChanges();
        }
        private ICollection<LineItemType> GetLineItemType()
        {
            ICollection<LineItemType> lineItemTypes = new List<LineItemType>()
            {
                new LineItemType(){Id=1,Title="اصلی",LineItemTypeGroupId=1,Description=""},
                new LineItemType(){Id=2,Title="جانبازان",LineItemTypeGroupId=4,Description=""},
                new LineItemType(){Id=3,Title="خانواده شهدا",LineItemTypeGroupId=4,Description=""},
                new LineItemType(){Id=4,Title="9%",LineItemTypeGroupId=2,Description=""},
                new LineItemType(){Id=5,Title="10%",LineItemTypeGroupId=2,Description=""},
                new LineItemType(){Id=6,Title="کارمزد ارزیابی",LineItemTypeGroupId=3,Description=""},
            };

            return lineItemTypes;
        }
    }
}
