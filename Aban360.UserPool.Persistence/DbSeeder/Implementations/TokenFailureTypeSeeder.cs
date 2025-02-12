using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Constants;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.DbSeeder.Implementations
{
    public class TokenFailureTypeSeeder : IDataSeeder
    {
        public int Order { get; set; } = 15;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<TokenFailureType> _tokenFailureTypes;
        public TokenFailureTypeSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _tokenFailureTypes=_uow.Set<TokenFailureType>();
            _tokenFailureTypes.NotNull(nameof(_tokenFailureTypes));
        }
        public void SeedData()
        {
            if (_tokenFailureTypes.Any())
            {
                return;
            }
            var tokenFailureTypes = new List<TokenFailureType>()
            {
                   new TokenFailureType(){ Id=TokenFailureTypeEnum.NoActionOrController,Title="مسیر پیدا نشد" },
                   new TokenFailureType(){ Id=TokenFailureTypeEnum.NoClaims,Title="توکن بدون claim صحیح" },
                   new TokenFailureType(){ Id=TokenFailureTypeEnum.NoSerial,Title="فیلد سریال پیدا نشد" },
                   new TokenFailureType(){ Id=TokenFailureTypeEnum.NoUserId,Title="فیلد آی دی کاربر یافت نشد" },
                   new TokenFailureType(){ Id=TokenFailureTypeEnum.Expired,Title="زمان اعتبار توکن منقضی شده" },
                   new TokenFailureType(){ Id=TokenFailureTypeEnum.NoTokenInDb,Title="توکن در پایگاه داده پیدا نشد" },
                   new TokenFailureType(){ Id=TokenFailureTypeEnum.NoAccess,Title="دسترسی غیرمجاز" },
                   new TokenFailureType(){ Id=TokenFailureTypeEnum.DeviceChanged,Title="دستگاه کاربر تغییر پیدا کرده" },
                   new TokenFailureType(){ Id=TokenFailureTypeEnum.InactiveSession,Title="مدت زمان session به پایان رسیده است" },
                   new TokenFailureType(){Id=TokenFailureTypeEnum.NotOurToken, Title="توکن متعلق به سرویس های ما نیست"}
            };
            _tokenFailureTypes.AddRange(tokenFailureTypes);
            _uow.SaveChanges();
        }
    }
}
