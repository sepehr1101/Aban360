using Aban360.UserPool.Domain.Features.AceessTree.Entites;

namespace Aban360.UserPool.Persistence.Features.UiElement.Commands.Implementations
{
    public interface IAppCommandService
    {
        Task Add(App app);
        void Remove(App app);
    }
}