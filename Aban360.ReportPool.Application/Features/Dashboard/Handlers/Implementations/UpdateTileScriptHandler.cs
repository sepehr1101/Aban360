using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Dashboard.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.Dashboard.Dtos;
using Aban360.ReportPool.Persistence.Features.Dashboard.Contracts;

namespace Aban360.ReportPool.Application.Features.Dashboard.Handlers.Implementations
{
    internal sealed class UpdateTileScriptHandler : IUpdateTileScriptHandler
    {
        private readonly ITileScriptService _tileScriptService;

        public UpdateTileScriptHandler(ITileScriptService service)
        {
            _tileScriptService = service;
            _tileScriptService.NotNull(nameof(_tileScriptService));
        }

        public async Task<bool> Handle(TileScriptDto dto, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _tileScriptService.Update(dto);
        }
    }
}
