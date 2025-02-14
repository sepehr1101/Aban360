using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Delete.Contracts
{
    public interface IIndividualEstateDeleteHandler
    {
        Task Handle(IndividualEstateDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
