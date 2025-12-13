using Aban360.Common.ApplicationUser;
using Aban360.ReportPool.Domain.Features.Dashboard.Dtos;

namespace Aban360.ReportPool.Application.Features.Dashboard.Handlers.Contracts
{
    public interface IGetReportByTileScriptContentHandler
    {
        Task<IEnumerable<TileScriptReportDto>> Handle(int reportCode, IAppUser appUser, CancellationToken cancellationToken);
    }
}