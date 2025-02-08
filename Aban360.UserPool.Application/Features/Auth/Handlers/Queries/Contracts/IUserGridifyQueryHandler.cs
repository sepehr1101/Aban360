using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Gridify;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts
{
    public interface IUserGridifyQueryHandler
    {
        Task<ICollection<UserQueryDto>> Handle(GridifyQuery query, CancellationToken cancellationToken);
    }
}