using Aban360.UserPool.Domain.Features.AceessTree.Entites;

namespace Aban360.UserPool.Persistence.Features.UiElement.Commands.Implementations
{
    public interface ISubModuleCommandService
    {
        Task Add(SubModule subModule);
        void Remove(SubModule subModule);
    }
}