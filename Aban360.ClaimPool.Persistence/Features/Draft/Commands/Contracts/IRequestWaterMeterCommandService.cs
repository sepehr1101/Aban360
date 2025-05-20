using Aban360.ClaimPool.Domain.Features.Draft.Entites;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts
{
    public interface IRequestWaterMeterCommandService
    {
        Task Add(RequestWaterMeter requestWaterMeter);
        void Remove(RequestWaterMeter requestWaterMeter);
    }
}
