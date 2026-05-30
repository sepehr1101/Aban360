using Aban260.BlobPool.Infrastructure.Providers.OpenKm.Contracts;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Querys.Contracts;
using Aban360.Common.Extensions;
using System.Runtime.InteropServices;

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

        public async Task<string> Handle(string folderName, CancellationToken cancellationToken, [Optional] string path)
        {
            string pathToCheck = string.IsNullOrWhiteSpace(path) ? folderName : $"{path}/{folderName}";
            bool doesFolderExist = await _openKmQueryService.CheckFolderExists(pathToCheck);
            if (!doesFolderExist)
            {
                string uuid = await _openKmQueryService.CreateFolder(folderName, path);
                await _openKmQueryService.MarkNodeAsMetadatable(uuid, false);
                return uuid;
            }
            return string.Empty;
        }
    }
}
