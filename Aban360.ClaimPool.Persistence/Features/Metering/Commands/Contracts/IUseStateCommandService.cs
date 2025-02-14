using Aban360.ClaimPool.Domain.Features.Metering.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts
{
    public interface IUseStateCommandService
    {
        Task Add(UseState useState);
        Task Remove(UseState useState);
    }
}
