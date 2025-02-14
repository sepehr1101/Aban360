using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts
{
    public interface IIndividualEstateRelationTypeGetSingelHandler
    {
        Task<IndividualEstateRelationTypeGetDto> Handle(short id, CancellationToken cancellationToken);
    }
}
