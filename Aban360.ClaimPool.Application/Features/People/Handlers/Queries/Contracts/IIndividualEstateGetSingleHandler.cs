using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts
{
    public interface IIndividualEstateGetSingleHandler
    {
        Task<IndividualEstateGetDto> Handle(int id, CancellationToken cancellationToken);
    }
}
