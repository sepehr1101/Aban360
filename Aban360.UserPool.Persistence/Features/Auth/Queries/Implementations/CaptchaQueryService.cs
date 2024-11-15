using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.Auth.Queries.Implementations
{
    public sealed class CaptchaQueryService : ICaptchaQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Captcha> _captchas;

        public CaptchaQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _captchas = _uow.Set<Captcha>();
            _captchas.NotNull(nameof(_captchas));
        }
        public async Task<Captcha> Get()
        {
            return await _captchas
                .Include(c=>c.CaptchaDisplayMode)
                .Include(c=>c.CaptchaLanguage)
                .FirstAsync();
        }
    }
}