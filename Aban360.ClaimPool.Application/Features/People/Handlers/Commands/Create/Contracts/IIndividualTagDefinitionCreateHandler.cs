using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Contracts
{
    public interface IIndividualTagDefinitionCreateHandler
    {
        Task Handle(IndividualTagDefinitionCreateDto createDto, CancellationToken cancellationToken);
    }
}
