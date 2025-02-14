using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Contracts
{
    public interface IIndividualUpdateHandler
    {
        Task Handle(IndividualUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
