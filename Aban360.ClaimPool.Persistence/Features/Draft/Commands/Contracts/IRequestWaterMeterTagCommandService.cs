using Aban360.ClaimPool.Domain.Features.Draft.Entites;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts
{
    public interface IRequestWaterMeterTagCommandService
    {
        Task Add(RequestWaterMeterTag requestWaterMeterTag);
        Task Remove(RequestWaterMeterTag requestWaterMeterTag);
        void Remove(ICollection<RequestWaterMeterTag> requestWaterMeterTags);
    }
}
