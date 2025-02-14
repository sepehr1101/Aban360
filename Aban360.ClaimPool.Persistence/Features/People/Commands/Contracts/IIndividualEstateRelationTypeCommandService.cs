using Aban360.ClaimPool.Domain.Features.People.Entities;

namespace Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts
{
    public interface IIndividualEstateRelationTypeCommandService
    {
        Task Add(IndividualEstateRelationType individualEstateRelationType);
        Task Remove(IndividualEstateRelationType individualEstateRelationType);
    }
}
