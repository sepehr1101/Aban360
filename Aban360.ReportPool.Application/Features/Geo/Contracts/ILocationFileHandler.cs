using Aban360.Common.ApplicationUser;

namespace Aban360.ReportPool.Application.Features.Geo.Contracts
{
    public interface ILocationFileHandler
    {
        Task<byte[]> Handle(string billId, IAppUser appUser, CancellationToken cancellationToken);
    }
}
