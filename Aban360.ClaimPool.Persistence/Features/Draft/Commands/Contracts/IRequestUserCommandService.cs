using Aban360.ClaimPool.Domain.Features.Draft.Entites;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts
{
    public interface IRequestUserCommandService
    {
        Task Add(RequestUser requestUser);
        Task Add(RequestWaterMeter requestWaterMeter);
    }
}
