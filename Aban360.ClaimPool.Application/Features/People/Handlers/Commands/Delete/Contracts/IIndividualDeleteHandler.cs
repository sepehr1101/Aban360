using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Delete.Contracts
{
    public interface IIndividualDeleteHandler
    {
        Task Handle(IndividualDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
