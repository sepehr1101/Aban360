// Test-only contract retained for pre-refactor snapshots.
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts
{
    public interface IIndividualEstateGetAllHandler
    {
        Task<ICollection<IndividualEstateGetDto>> Handle(CancellationToken cancellationToken);
    }
}
