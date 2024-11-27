using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.Auth.Commands.Implementations
{
    public sealed class CaptchaCommandService : ICaptchaCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Captcha> _captchas;

        public CaptchaCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _captchas = _uow.Set<Captcha>();
            _captchas.NotNull(nameof(_captchas));
        }

        public async Task Create(Captcha captcha)
        {
            await _captchas.AddAsync(captcha);
        }
        public void Delete(Captcha captcha)
        {
            _captchas.Remove(captcha);
        }
        public void Update(Captcha captcha)
        {
            _uow.SetEntityState(captcha, EntityState.Modified);
            //var captchaInDb = await _captchas.FindAsync(captcha.Id);
        }
        public async Task SetIsSelected(int id)
        {
            var captchas = await _captchas.ToListAsync();
            foreach (var captcha in captchas)
            {
                captcha.IsSelected = false;
            }
            var targetCaptcha = await _captchas.FindAsync(id);
            targetCaptcha.IsSelected = true;
        }
    }
}