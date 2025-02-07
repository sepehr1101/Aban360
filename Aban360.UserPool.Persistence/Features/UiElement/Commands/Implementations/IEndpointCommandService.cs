using Aban360.UserPool.Domain.Features.AceessTree.Entites;

namespace Aban360.UserPool.Persistence.Features.UiElement.Commands.Implementations
{
    public interface IEndpointCommandService
    {
        Task Add(Endpoint endpoint);
        void Remove(Endpoint endpoint);
    }
}