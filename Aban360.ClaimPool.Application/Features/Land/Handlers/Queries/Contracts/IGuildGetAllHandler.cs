using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts
{
    public interface IGuildGetAllHandler
    {
        Task<ICollection<GuildGetDto>> Handle(CancellationToken cancellationToken);
    }
}
