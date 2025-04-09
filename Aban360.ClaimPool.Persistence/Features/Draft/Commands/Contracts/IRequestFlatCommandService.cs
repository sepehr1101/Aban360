using Aban360.ClaimPool.Domain.Features.Draft.Entites;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts
{
    public interface IRequestFlatCommandService
    {
        Task Add(RequestFlat requestFlat);
        Task Remove(RequestFlat requestFlat);
    }
}
