using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Persistence.Features.WasteWater.Commands.Contracts
{
    public interface ISiphonCommandService
    {
        Task Add(Siphon siphon);
        Task Remove(Siphon siphon);
    }
}
