using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.AceessTree.Entites;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.UiElement.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.UiElement.Queries.Implementations
{
    public sealed class SubModuleCommandService : ISubModuleQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<SubModule> _subModules;
        public SubModuleCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _subModules = _uow.Set<SubModule>();
            _subModules.NotNull(nameof(_subModules));
        }
        public IQueryable<SubModule> GetQuery()
        {
            return _subModules.AsQueryable();
        }
        public async Task<ICollection<SubModule>> Get()
        {
            return await _subModules
                .Where(subModule => subModule.IsActive)
                .ToListAsync();
        }
        public async Task<ICollection<SubModule>> GetInclude()
        {
            return await _subModules
                .Include(subModule => subModule.Module)
                .Where(subModule => subModule.IsActive)
                .ToListAsync();
        }
        public async Task<SubModule> Get(int id)
        {
            return await _uow.FindOrThrowAsync<SubModule>(id);
        }
        public async Task<SubModule> GetInclude(int id)
        {
            return await _subModules
                .Include(subModule => subModule.Module)
                .Where(subModule => subModule.Id == id)
                .SingleAsync();
        }
    }
}
