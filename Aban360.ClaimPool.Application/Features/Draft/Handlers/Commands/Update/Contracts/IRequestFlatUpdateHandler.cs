using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts
{
    public interface IRequestFlatUpdateHandler
    {
        Task Handle(FlatRequestUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
