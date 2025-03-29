using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;

namespace Aban360.ClaimPool.Persistence.Features.WasteWater.Commands.Contracts
{
    public interface ISiphonCommandService
    {
        Task Add(Siphon siphon);
        Task Add(ICollection<Siphon> siphons);
        Task Remove(Siphon siphon);
    }
}
