using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.AceessTree.Entites;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.UiElement.Queries.Implementations
{
    public sealed class ModuleCommandService : IModuleQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Module> _modules;
        public ModuleCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _modules = _uow.Set<Module>();
            _modules.NotNull(nameof(_modules));
        }
        public IQueryable<Module> GetQuery()
        {
            return _modules.AsQueryable();
        }
        public async Task<ICollection<Module>> Get()
        {
            return await _modules
                .Where(module => module.IsActive)
                .ToListAsync();
        }
        
        public async Task<ICollection<Module>> GetInclude()
        {
            return await _modules
                .Include(module => module.App)
                .Where(module => module.IsActive)
                .ToListAsync();
        }
        public async Task<Module> Get(int id)
        {
            return await _uow.FindOrThrowAsync<Module>(id);
        }
        
        public async Task<Module> GetInclude(int id)
        {
            return await _modules
                .Include(m => m.App)
                .Where(m => m.Id == id)
                .SingleAsync();
        }
    }
}
