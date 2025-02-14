using Aban360.ClaimPool.Domain.Features.Registration.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Registration.Handlers.Queries.Contracts
{
    public interface IUseEstateGetSingleHandler
    {
        Task<UseStateGetDto> Handle(short id, CancellationToken cancellationToken);
    }
}
