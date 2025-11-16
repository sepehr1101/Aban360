using Aban360.ReportPool.Domain.Features.Geo;

namespace Aban360.ReportPool.Application.Features.Geo.Contracts
{
    public interface ILocationInfoGetHandler
    {
        Task<LocationInfoDto> Handle(string billId, CancellationToken cancellationToken);
    }
}
