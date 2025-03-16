using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.Auth.Queries.Implementations
{
    internal sealed class CaptchaDisplayModeQueryService : ICaptchaDisplayModeQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<CaptchaDisplayMode> _captchaDisplayModes;
        public CaptchaDisplayModeQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _captchaDisplayModes = _uow.Set<CaptchaDisplayMode>();
            _captchaDisplayModes.NotNull(nameof(_captchaDisplayModes));
        }
        public async Task<ICollection<CaptchaDisplayMode>> Get()
        {
            return await _captchaDisplayModes.ToListAsync();
        }

        public async Task<CaptchaDisplayMode> Get(short id)
        {
            return await _uow.FindOrThrowAsync<CaptchaDisplayMode>(id);
        }
    }
}