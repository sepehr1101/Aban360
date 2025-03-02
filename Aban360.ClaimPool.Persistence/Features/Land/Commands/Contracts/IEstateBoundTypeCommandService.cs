using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts
{
    public interface IEstateBoundTypeCommandService
    {
        Task Add(EstateBoundType estateBoundType);
        Task Remove(EstateBoundType estateBoundType);
    }
}
