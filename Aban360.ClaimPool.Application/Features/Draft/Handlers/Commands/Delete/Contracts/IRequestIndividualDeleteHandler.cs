using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Contracts
{
    public interface IRequestIndividualDeleteHandler
    {
        Task Handle(IndividualRequestDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
