using Aban360.Common.ApplicationUser;
using Aban360.ReportPool.Domain.Features.Geo;

namespace Aban360.ReportPool.Application.Features.Geo.Contracts
{
    public interface ILocationBase64GetHandler
    {
        Task<LocationBase64Dto> Handle(string billId, IAppUser appUser, CancellationToken cancellationToken);
    }
}
