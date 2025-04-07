using Aban360.ClaimPool.Domain.Features.Draft.Entites;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts
{
    public interface IRequestSiphonCommandService
    {
        Task Add(RequestSiphon requestSiphon);
        Task Remove(RequestSiphon requestSiphon);
    }
}
