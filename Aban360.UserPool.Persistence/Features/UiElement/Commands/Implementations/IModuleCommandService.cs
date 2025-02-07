using Aban360.UserPool.Domain.Features.AceessTree.Entites;

namespace Aban360.UserPool.Persistence.Features.UiElement.Commands.Implementations
{
    public interface IModuleCommandService
    {
        Task Add(Module module);
        void Remove(Module module);
    }
}