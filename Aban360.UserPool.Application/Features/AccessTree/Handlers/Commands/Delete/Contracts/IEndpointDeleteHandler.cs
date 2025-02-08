using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;

namespace Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Delete.Contracts
{
    public interface IEndpointDeleteHandler
    {
        Task Handle(EndpointDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
