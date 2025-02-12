using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts
{
    public interface IFlatCommandService
    {
        Task Add(Flat flat);
        Task Remove(Flat flat);
    }
}
