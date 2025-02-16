using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Contracts
{
    public interface IIndividualTagDefinitionUpdateHandler
    {
        Task Handle(IndividualTagDefinitionUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
