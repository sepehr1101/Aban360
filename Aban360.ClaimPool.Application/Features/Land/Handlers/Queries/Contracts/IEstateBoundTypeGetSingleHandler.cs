using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts
{
    public interface IEstateBoundTypeGetSingleHandler
    {
        Task<EstateBoundTypeGetDto> Handle(short id, CancellationToken cancellationToken);
    }
}
