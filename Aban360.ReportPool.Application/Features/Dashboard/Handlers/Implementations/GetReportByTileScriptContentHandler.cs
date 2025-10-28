using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Dashboard.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.Dashboard.Dtos;
using Aban360.ReportPool.Domain.Features.Dashboard.Entities;
using Aban360.ReportPool.Persistence.Features.Dashboard.Contracts;

namespace Aban360.ReportPool.Application.Features.Dashboard.Handlers.Implementations
{
    internal sealed class GetReportByTileScriptContentHandler : IGetReportByTileScriptContentHandler
    {
        private readonly ITileScriptService _tileScriptService;

        public GetReportByTileScriptContentHandler(ITileScriptService service)
        {
            _tileScriptService = service;
            _tileScriptService.NotNull(nameof(_tileScriptService));
        }

        public async Task<IEnumerable<TileScriptReportDto>> Handle(int id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            TileScript tileScript = await _tileScriptService.GetById(id);
            if (tileScript != null && tileScript.Content != null)
            {
                IEnumerable<TileScriptReportDto> report = await _tileScriptService.GetContent(tileScript.Content);
                return report;

            }

            return null;
        }
    }
}