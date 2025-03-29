using Aban360.ClaimPool.Domain.Features.People.Entities;

namespace Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts
{
    public interface IIndividualEstateCommandService
    {
        Task Add(IndividualEstate individualEstate);
        Task Remove(IndividualEstate individualEstate);
    }
}
