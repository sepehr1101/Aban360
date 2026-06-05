using Aban260.BlobPool.Infrastructure.Providers.OpenKm.Contracts;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts;
using Aban360.BlobPool.Domain.Features.OpenKm;
using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.Common.Extensions;

namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Implementations
{
    internal sealed class DisplayRemovedFilesHandler : IDisplayRemovedFilesHandler
    {
        private readonly IOpenKmQueryService _openKmQueryService;
        public DisplayRemovedFilesHandler(IOpenKmQueryService openKmQueryService)
        {
            _openKmQueryService = openKmQueryService;
            _openKmQueryService.NotNull(nameof(openKmQueryService));
        }

        public async Task<FileListResponse> Handle(RemovedFilesInput input, CancellationToken cancellationToken)
        {
            string directory = input.IsBillId ? input.FolderName : $"r_{input.FolderName}";
            FileListResponse result = await _openKmQueryService.GetRemovedFiles(directory);
            return result;
        }
    }
}
