using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.AceessTree.Entites;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.UiElement.Commands.Implementations
{
    public sealed class ModuleCommandService : IModuleCommandService
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
        public async Task Add(Module module)
        {
            await _modules.AddAsync(module);
        }
        public void Remove(Module module)
        {
            module.IsActive = false;
        }
    }
}
