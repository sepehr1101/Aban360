using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts
{
    public interface IProfessionCommandService
    {
        Task Add(Profession profession);
        Task Remove(Profession profession);
    }
}
