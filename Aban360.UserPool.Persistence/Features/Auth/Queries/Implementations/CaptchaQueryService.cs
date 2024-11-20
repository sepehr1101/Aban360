using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.Auth.Queries.Implementations
{
    public sealed class CaptchaCommandService : ICaptchaQueryService
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
        public async Task<Captcha> Get()
        {
            return await _captchas
                .AsNoTracking()
                .Include(c=>c.CaptchaDisplayMode)
                .Include(c=>c.CaptchaLanguage)
                .Where(c=>c.IsSelected)
                .SingleAsync();
        }

        public async Task<ICollection<Captcha>> GetAll()
        {
            return await _captchas
                .AsNoTracking()
                .Include(c => c.CaptchaDisplayMode)
                .Include(c => c.CaptchaLanguage)
                .ToListAsync();
        }
    }
}