using Aban360.ClaimPool.Domain.Features.Draft.Entites;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts
{
    public interface IRequestEstateCommandService
    {
        Task Add(IEnumerable<RequestEstate> requestEstates);
        Task Add(RequestEstate requestEstate);
        void Remove(RequestEstate requestEstate);
    }
}