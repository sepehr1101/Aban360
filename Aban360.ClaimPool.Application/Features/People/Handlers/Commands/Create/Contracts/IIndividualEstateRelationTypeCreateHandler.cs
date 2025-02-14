using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Contracts
{
    public interface IIndividualEstateRelationTypeCreateHandler
    {
        Task Handle(IndividualEstateRelationTypeCreateDto createDto, CancellationToken cancellationToken);
    }
}
