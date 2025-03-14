using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts
{
    public interface IGetewayGetAllHandler
    {
        Task<ICollection<GetewayGetDto>> Handle(CancellationToken cancellationToken);
    }
}