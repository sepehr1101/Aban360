using Aban360.ClaimPool.Domain.Features.Request.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Request.Commands.Contracts
{
    public interface IGetewayCommandService
    {
        Task Add(Geteway geteway);
        Task Remove(Geteway geteway);
    }
}