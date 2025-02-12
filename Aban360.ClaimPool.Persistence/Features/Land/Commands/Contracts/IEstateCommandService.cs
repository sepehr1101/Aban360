using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts
{
    public interface IEstateCommandService
    {
        Task Add(Estate estate);
        Task Remove(Estate estate);
    }
}
