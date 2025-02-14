using Aban360.ClaimPool.Domain.Features.Registration.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Registration.Commands.Contracts
{
    public interface IUseStateCommandService
    {
        Task Add(UseState useState);
        Task Remove(UseState useState);
    }
}
