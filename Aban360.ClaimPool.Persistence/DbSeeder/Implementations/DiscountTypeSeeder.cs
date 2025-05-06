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
                new DiscountType(){Id=DiscountTypeEnum.BedonTakhfif,Title="بدون تخفیف"},
                new DiscountType(){Id=DiscountTypeEnum.KhanevadeShohada,Title="خانواده شهدا"},
                new DiscountType(){Id=DiscountTypeEnum.Janbazan,Title="جانبازان"},
                new DiscountType(){Id=DiscountTypeEnum.Basij,Title="بسیج"},
                new DiscountType(){Id=DiscountTypeEnum.Behzisti ,Title="بهزیستی"},
                new DiscountType(){Id=DiscountTypeEnum.KomiteEmdad,Title="کمیته امداد"},
                new DiscountType(){Id=DiscountTypeEnum.Masajed,Title="مساجد"},
                new DiscountType(){Id=DiscountTypeEnum.Razmandegan,Title="رزمندگان"},
                new DiscountType(){Id=DiscountTypeEnum.AmakenMazhabi,Title="اماکن مذهبی"},
                new DiscountType(){Id=DiscountTypeEnum.MarakezAmozeshi,Title="مراکز آموزشی"},
                new DiscountType(){Id=DiscountTypeEnum.JavaniJamiyat1,Title="جوانی جمعیت 1"},
                new DiscountType(){Id=DiscountTypeEnum.HeyatModire,Title="هیئت مدیره"},
                new DiscountType(){Id=DiscountTypeEnum.PadashNaghdi,Title="پاداش نقدی"},
                new DiscountType(){Id=DiscountTypeEnum.FarzandJanbaz70Darsad,Title="فرزند جانباز70 درصد"},
                new DiscountType(){Id=DiscountTypeEnum.KhanevadeJanbazVaAzadeMotovafi,Title="خانواده جانباز و آزاده متوفی"},
                new DiscountType(){Id=DiscountTypeEnum.JavaniJamiyat2,Title="جوانی جمعیت 2"},
                new DiscountType(){Id=DiscountTypeEnum.KhayerinMaskanSaz,Title="خیرین مسکن ساز"},
            };
            _discountType.AddRange(discountType);
            _uow.SaveChanges();
        }
    }
}
