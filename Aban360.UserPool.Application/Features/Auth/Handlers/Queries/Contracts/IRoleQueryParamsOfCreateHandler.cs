using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts
{
    public interface IRoleQueryParamsOfCreateHandler
    {
        Task<RoleParamsOfCreateDto> Handle(CancellationToken cancellationToken);
    }
}