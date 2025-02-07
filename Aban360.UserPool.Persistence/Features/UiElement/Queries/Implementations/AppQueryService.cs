using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.AceessTree.Entites;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.UiElement.Queries.Implementations
{
    public sealed class AppCommandService : IAppQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<App> _apps;
        public AppCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _apps = _uow.Set<App>();
            _apps.NotNull(nameof(_apps));
        }
        public IQueryable<App> GetQuery()
        {
            return _apps.AsQueryable();
        }
        public async Task<ICollection<App>> Get()
        {
            return await _apps
                .Where(app => app.IsActive)
                .ToListAsync();
        }
        public async Task<App> Get(int id)
        {
            return await _uow.FindOrThrowAsync<App>(id);
        }
    }
}
