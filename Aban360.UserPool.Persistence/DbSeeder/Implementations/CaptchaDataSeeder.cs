using Microsoft.EntityFrameworkCore;
using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.DbSeeder.Contracts;

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
                BackColor = "",
                CaptchaDisplayModeId = 1,
                CaptchaLanguageId = 1,
                EncryptionKey = "This is my secret",
                ExpiresAfter = 7,
                FontName = "Tahoma",
                FontSize = 12,
                ForeColor = "#111111",
                Identifier = "Aban360",
                HiddenInputName = "hidden",
                InputClass = "class-1",
                InputName = "inputName",
                InputPlaceholder = "placeHolder",
                InputTemplate = "<input>",
                Noise = "asdfas",
                NonceKey = "nonceKey",
                RateLimit = 10,
                RateLimitMessage = "ignored rate limit mate",
                RefreshButtonClass = "refresh",
                ShowRefreshButton = true,
                ShowThousandSeperator = true,
                ValidationMessage = "invalid",
                ValidationMessageClass = "validation-class",
                HiddenTokenName="hidden token Name"
            };       
            _capthcas.Add(captcha1);
        }
    }
}
