using Aban360.ClaimPool.Domain.Features.Draff.Entites;

namespace Aban360.ClaimPool.Persistence.Features.Draff.Commands.Contracts
{
    public interface IRequestUserCommandService
    {
        Task Add(RequestUser requestUser);
    }
}
