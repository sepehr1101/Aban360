using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts
{
    public interface IUserBriefQueryHandler
    {
        Task<ICollection<UserBriefQueryDto>> Handle(CancellationToken cancellationToken);
    }
}
