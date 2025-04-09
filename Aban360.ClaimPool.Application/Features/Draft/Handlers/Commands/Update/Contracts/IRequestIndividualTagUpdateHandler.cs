using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts
{
    public interface IRequestIndividualTagUpdateHandler
    {
        Task Handle(IndividualTagRequestUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
