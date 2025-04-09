using Aban360.ClaimPool.Domain.Features.Draft.Entites;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts
{
    public interface IRequestIndividualCommandService
    {
        Task Add(RequestIndividual requestIndividual);
        Task Remove(RequestIndividual requestIndividual);
    }
}
