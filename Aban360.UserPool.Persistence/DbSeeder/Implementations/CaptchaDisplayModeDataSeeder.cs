using Microsoft.EntityFrameworkCore;
using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.Common.Db.DbSeeder.Contracts;

namespace SmsHub.Persistence.DbSeeder.Implementations
{
    public class CaptchaDisplayModeDataSeeder : IDataSeeder
    {       
        private readonly IUnitOfWork _uow;
        private readonly DbSet<CaptchaDisplayMode> _capthcaDisplayModes;
        public CaptchaDisplayModeDataSeeder(IUnitOfWork uow)
        {          
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _capthcaDisplayModes = _uow.Set<CaptchaDisplayMode>();
        }
        public int Order { get; set; } = 2;

        public void SeedData()
        {
            if (_capthcaDisplayModes.Any())
            {
                return;
            }
            var captcahDisplayModes = new List<CaptchaDisplayMode>()
            {
                new CaptchaDisplayMode(){Id=1, DisplayModeEnumId=0,Name="NumberToWord",Tite="تبدیل حروف به ارقام"},
                new CaptchaDisplayMode(){Id=2,DisplayModeEnumId=1,Name="ShowDigits",Tite="نمایش ارقام"},
                new CaptchaDisplayMode(){Id=3,DisplayModeEnumId=2,Name="SumOfTwoNumbers",Tite="جمع دو عدد"},
                new CaptchaDisplayMode(){Id=4,DisplayModeEnumId=3,Name="SumOfTwoNumbersToWords",Tite="جمع دو عدد با حروف"}
            };
            _capthcaDisplayModes.AddRange(captcahDisplayModes);
            _uow.SaveChanges();
        }
    }
}
