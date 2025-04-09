using Aban360.ClaimPool.Domain.Features.Draft.Entites;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts
{
    public interface IRequestIndividualEstateCommandService
    {
        Task Add(RequestIndividualEstate requestIndividualEstate);
        Task Remove(RequestIndividualEstate requestIndividualEstate);
    }
}
