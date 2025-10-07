using Aban360.Common.ApplicationUser;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Dashboard.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.Dashboard.Dtos;
using Aban360.ReportPool.Domain.Features.Dashboard.Entities;
using Aban360.ReportPool.Persistence.Features.Dashboard.Contracts;

namespace Aban360.ReportPool.Application.Features.Dashboard.Handlers.Implementations
{
    internal sealed class CreateTileScriptHandler : ICreateTileScriptHandler
    {
        private readonly ITileScriptService _tileScriptService;

        public CreateTileScriptHandler(ITileScriptService service)
        {
            _tileScriptService = service;
            _tileScriptService.NotNull(nameof(_tileScriptService));
        }

        public async Task<int> Handle(TileScriptDto dto, IAppUser currentUser, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _tileScriptService.Create(Map(dto, currentUser));
        }
        private TileScript Map(TileScriptDto dto, IAppUser currentUser)
        {
            return new TileScript()
            {
                Content = dto.Content,
                CreateDateTime = DateTime.Now,
                CreatedBy = currentUser.FullName,
                Description = dto.Description,
                ReportCode = dto.ReportCode,
            };
        }
    }
}
