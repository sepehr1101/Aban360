using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.AceessTree.Entites;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.UiElement.Commands.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.UiElement.Commands.Implementations
{
    public sealed class SubModuleCommandService : ISubModuleCommandService
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
        public async Task Add(SubModule subModule)
        {
            await _subModules.AddAsync(subModule);
        }
        public void Remove(SubModule subModule)
        {
            subModule.IsActive = false;
        }
    }
}
