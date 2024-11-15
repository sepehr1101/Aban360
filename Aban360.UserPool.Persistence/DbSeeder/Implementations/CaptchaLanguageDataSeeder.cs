using Microsoft.EntityFrameworkCore;
using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.DbSeeder.Contracts;
using System.ComponentModel.DataAnnotations;

namespace SmsHub.Persistence.DbSeeder.Implementations
{
    public class CaptchaLanguageDataSeeder : IDataSeeder
    {       
        private readonly IUnitOfWork _uow;
        private readonly DbSet<CaptchaLanguage> _captchLanguages;
        public CaptchaLanguageDataSeeder(IUnitOfWork uow)
        {          
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _captchLanguages=_uow.Set<CaptchaLanguage>();

        }
        public int Order { get; set; } = 1;

        public void SeedData()
        {
            if (_captchLanguages.Any())
            {
                return;
            }
            var captchaLanguages = new List<CaptchaLanguage>()
            {
                new CaptchaLanguage() {Id = 1, LanguageId=1, Name = "Persian", Title="فارسی" },
                new CaptchaLanguage() {Id = 2, LanguageId=0, Name = "English", Title = "English" },
                new CaptchaLanguage() {Id = 3, LanguageId=4, Name = "Turkish", Title = "Türkçe" },
                new CaptchaLanguage() {Id = 4, LanguageId=5, Name = "Arabic", Title = "عربي" }
            };
            _captchLanguages.AddRange(captchaLanguages);
            _uow.SaveChanges();
        }
    }
}
