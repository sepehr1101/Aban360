using Aban260.BlobPool.Infrastructure.Providers.OpenKm.Contracts;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts;
using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.Common.Extensions;

namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Implementations
{
    internal sealed class AddFileHandler : IAddFileHandler
    {
        private readonly IOpenKmQueryService _openKmQueryService;
        public AddFileHandler(IOpenKmQueryService openKmQueryService)
        {
            _openKmQueryService = openKmQueryService;
            _openKmQueryService.NotNull(nameof(openKmQueryService));
        }

        public async Task<AddFileDto> Handle(string serverPath, string localFilePath, CancellationToken cancellationToken)
        {
            return await _openKmQueryService.AddFile(serverPath, localFilePath);
        }
    }
}
