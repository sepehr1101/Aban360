using Aban360.ClaimPool.Domain.Features.Draft.Entites;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts
{
    public interface IRequestWaterMeterSiphonCommandService
    {
        Task Add(RequestWaterMeterSiphon requestWaterMeterSiphon);
        void Remove(RequestWaterMeterSiphon requestWaterMeterSiphon);
    }
}
