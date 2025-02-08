using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;

namespace Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Contracts
{
    public interface IEndpointGetSingleHandler
    {
        Task<EndpointGetDto> Handle(int id,CancellationToken cancellationToken);
    }
}
