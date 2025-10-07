using Aban360.Common.ApplicationUser;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Dashboard.Handlers.Contracts;
using Aban360.ReportPool.Persistence.Features.Dashboard.Contracts;

namespace Aban360.ReportPool.Application.Features.Dashboard.Handlers.Implementations
{
    internal sealed class DeleteTileScriptHandler : IDeleteTileScriptHandler
    {
        private readonly ITileScriptService _tileScriptService;

        public DeleteTileScriptHandler(ITileScriptService service)
        {
            _tileScriptService = service;
            _tileScriptService.NotNull(nameof(_tileScriptService));
        }

        public async Task<bool> Handle(int id, IAppUser currentUser, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _tileScriptService.Delete(id, currentUser.FullName);
        }
    }
}
