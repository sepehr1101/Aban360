using Microsoft.EntityFrameworkCore;
using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.Common.Db.DbSeeder.Contracts;

namespace SmsHub.Persistence.DbSeeder.Implementations
{
    public class CaptchaDataSeeder : IDataSeeder
    {       
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Captcha> _capthcas;
        public CaptchaDataSeeder(IUnitOfWork uow)
        {          
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _capthcas = _uow.Set<Captcha>();
            _capthcas.NotNull(nameof(_capthcas));
        }
        public int Order { get; set; } = 3;

        public void SeedData()
        {
            if (_capthcas.Any())
            {
                return;
            }
            var captcha1 = new Captcha()
            {
                BackColor = "#f7f3f3",
                CaptchaDisplayModeId = 4,
                CaptchaLanguageId = 1,
                EncryptionKey = "This is my secret",
                ExpiresAfter = 7,
                FontName = "Tahoma",
                FontSize = 18,
                ForeColor = "#111111",
                //Identifier = "Aban360",
                //HiddenInputName = "hidden",
                //InputClass = "class-1",
                //InputName = "inputName",
                //InputPlaceholder = "placeHolder",
                //InputTemplate = "<input>",
                Noise = "putNoiserAsYouCan",
                NonceKey = "nonceKey",
                RateLimit = 10,
                //RateLimitMessage = "ignored rate limit mate",
                //RefreshButtonClass = "refresh",
                //ShowRefreshButton = true,
                ShowThousandSeperator = true,
                Direction="ltr",
                Min=0,
                Max=99,
                IsSelected=false,
                Title="کپچا فارسی جمع دو عدد به حروف"
            };
            var captcha2 = new Captcha()
            {
                BackColor = "#f7f3f3",
                CaptchaDisplayModeId = 3,
                CaptchaLanguageId = 1,
                EncryptionKey = "This is my secret",
                ExpiresAfter = 7,
                FontName = "Tahoma",
                FontSize = 18,
                ForeColor = "#111111",
                //Identifier = "Aban360",
                //HiddenInputName = "hidden",
                //InputClass = "class-1",
                //InputName = "inputName",
                //InputPlaceholder = "placeHolder",
                //InputTemplate = "<input>",
                Noise = "putNoiserAsYouCan",
                NonceKey = "nonceKey",
                RateLimit = 10,
                //RateLimitMessage = "ignored rate limit mate",
                //RefreshButtonClass = "refresh",
                //ShowRefreshButton = true,
                ShowThousandSeperator = true,
                Direction = "ltr",
                Min = 0,
                Max = 99,
                Title = "کپچا فارسی جمع دو عدد",
                IsSelected = true        
            };
            _capthcas.Add(captcha1);
            _capthcas.Add(captcha2);
            _uow.SaveChanges();
        }
    }
}
