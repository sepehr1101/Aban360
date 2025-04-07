using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Contracts
{
    public interface IRequestIndividualTagDeleteHandler
    {
        Task Handle(IndividualTagRequestDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
