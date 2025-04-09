using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts
{
    public interface IRequestIndividualTagCreateHandler
    {
        Task Handle(IndividualTagRequestCreateDto createDto, CancellationToken cancellationToken);
    }
}
