using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries;

namespace Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Contracts
{
    public interface IAppGetAllHandler
    {
        Task<ICollection<AppGetDto>> Handle(CancellationToken cancellationToken);
    }
}
