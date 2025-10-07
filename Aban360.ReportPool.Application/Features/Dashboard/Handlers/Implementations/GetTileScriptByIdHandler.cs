using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Dashboard.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.Dashboard.Entities;
using Aban360.ReportPool.Persistence.Features.Dashboard.Contracts;

namespace Aban360.ReportPool.Application.Features.Dashboard.Handlers.Implementations
{
    internal sealed class GetTileScriptByIdHandler : IGetTileScriptByIdHandler
    {
        private readonly ITileScriptService _tileScriptService;

        public GetTileScriptByIdHandler(ITileScriptService service)
        {
            _tileScriptService = service;
            _tileScriptService.NotNull(nameof(_tileScriptService));
        }

        public async Task<TileScript?> Handle(int id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _tileScriptService.GetById(id);
        }
    }
}
