using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries.ValueKeyItems;

namespace Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Contracts
{
    public interface IAccessTreeValueKeyQueryHandler
    {
        Task<AccessTreeValueKeyDto> Handle(CancellationToken cancellationToken);
        Task<AccessTreeValueKeyDto> Handle(ICollection<int> selectedEndpointIds, CancellationToken cancellationToken);
    }
}