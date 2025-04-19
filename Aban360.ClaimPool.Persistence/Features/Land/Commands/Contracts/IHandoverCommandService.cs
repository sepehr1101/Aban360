using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts
{
    public interface IHandoverCommandService
    {
        Task Add(Handover handover);
        Task Remove(Handover handover);
    }
}
