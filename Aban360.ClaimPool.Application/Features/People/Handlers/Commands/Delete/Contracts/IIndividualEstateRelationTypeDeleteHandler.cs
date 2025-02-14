using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Delete.Contracts
{
    public interface IIndividualEstateRelationTypeDeleteHandler
    {
        Task Handle(IndividualEstateRelationTypeDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
