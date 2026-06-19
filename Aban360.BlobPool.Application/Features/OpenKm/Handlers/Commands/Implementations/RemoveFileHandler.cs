using Aban260.BlobPool.Infrastructure.Providers.OpenKm.Contracts;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Querys.Contracts;
using Aban360.BlobPool.Domain.Features.OpenKm;
using Aban360.Common.Extensions;

namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Implementations
{
    internal sealed class RemoveFileHandler : IRemoveFileHandler
    {
        private readonly ICreateFolderHandler _createFolderHandler;
        private readonly IOpenKmQueryService _openKmQueryService;

        private const string deleteFolderName = "deleted";
        public RemoveFileHandler(
            ICreateFolderHandler createFolderHandler,
            IOpenKmQueryService openKmQueryService)
        {
            _createFolderHandler = createFolderHandler;
            _createFolderHandler.NotNull(nameof(createFolderHandler));

            _openKmQueryService = openKmQueryService;
            _openKmQueryService.NotNull(nameof(openKmQueryService));
        }
        public async Task Handle(RemoveFileDto removeFileDto, CancellationToken cancellationToken)
        {
            string directory = removeFileDto.IsBillId ? removeFileDto.FolderName : $"r_{removeFileDto.FolderName}";
            try
            {
                await _createFolderHandler.Handle(deleteFolderName, cancellationToken, $"{directory}");
            }
            catch
            {
                //برای مواردی که شناسه قبض موجود است اما هنوز مشترک قطعی نشده، بنابراین باید الزاماً با شماره پیگیری پیدا شود
                directory = $"r_{removeFileDto.TrackNumber}";
                await _createFolderHandler.Handle(deleteFolderName, cancellationToken, directory);
            }
            await _openKmQueryService.Move(removeFileDto.Uuid, $"{directory}/{deleteFolderName}");
        }
    }
}
