using Aban360.ClaimPool.Domain.Features.Request.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Request.Commands.Contracts
{
    public interface IGatewayCommandService
    {
        Task Add(Gateway geteway);
        Task Remove(Gateway geteway);
    }
}