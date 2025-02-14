using Aban360.ClaimPool.Domain.Features.Registration.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Registration.Handlers.Queries.Contracts
{
    public interface IUseEstateGetAllHandler
    {
        Task<ICollection<UseStateGetDto>> Handle(CancellationToken cancellationToken);
    }
}
