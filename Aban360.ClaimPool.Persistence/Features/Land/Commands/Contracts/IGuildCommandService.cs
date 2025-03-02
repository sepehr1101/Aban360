using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts
{
    public interface IGuildCommandService
    {
        Task Add(Guild guild);
        Task Remove(Guild guild);
    }
}
