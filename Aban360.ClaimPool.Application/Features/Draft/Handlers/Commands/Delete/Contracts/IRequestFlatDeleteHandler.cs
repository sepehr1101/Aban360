using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Contracts
{
    public interface IRequestFlatDeleteHandler
    {
        Task Handle(FlatRequestDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
