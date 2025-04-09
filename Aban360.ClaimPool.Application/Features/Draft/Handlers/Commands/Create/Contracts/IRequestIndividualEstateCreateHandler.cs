using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts
{
    public interface IRequestIndividualEstateCreateHandler
    {
        Task Handle(IndividualEstateRequestCreateDto createDto, CancellationToken cancellationToken);
    }
}
