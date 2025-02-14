using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Contracts
{
    public interface IIndividualEstateRelationTypeUpdateHandler
    {
        Task Handle(IndividualEstateRelationTypeUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
