using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts
{
    public interface IGuildGetSingleHandler
    {
        Task<GuildGetDto> Handle(short id, CancellationToken cancellationToken);
    }
}
