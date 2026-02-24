using Aban260.BlobPool.Infrastructure.Providers.OpenKm.Contracts;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Querys.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Implementations
{
    internal sealed class CreateFolderHandler : ICreateFolderHandler
    {
        private readonly IOpenKmQueryService _openKmQueryService;
        public CreateFolderHandler(IOpenKmQueryService openKmQueryService)
        {
            _openKmQueryService = openKmQueryService;
            _openKmQueryService.NotNull(nameof(openKmQueryService));
        }

        public async Task<string> Handle(string folderName, CancellationToken cancellationToken)
        {
            return await _openKmQueryService.CreateFolder(folderName);
        }
    }
}
