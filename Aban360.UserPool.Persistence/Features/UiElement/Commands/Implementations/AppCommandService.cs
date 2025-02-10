using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.AceessTree.Entites;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.UiElement.Commands.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.UiElement.Commands.Implementations
{
    public sealed class AppCommandService : IAppCommandService
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
        public async Task Add(App app)
        {
            await _apps.AddAsync(app);
        }
        public void Remove(App app)
        {
            app.IsActive = false;
        }
    }
}
