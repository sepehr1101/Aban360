using Aban260.BlobPool.Infrastructure.Features.DmsServices.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.BlobPool.Application.Features.DmsServices.Handlers.Commands.Create.Contracts
{
    public interface IFileInFolderCreateHandler
    {
        Task Handle(string serverPath, string localFilePath);
    }
    internal sealed class FileInFolderCreateHandler : IFileInFolderCreateHandler
    {
        private readonly IFileInFolderCreateServices _fileInFolderCreateServices;
        public FileInFolderCreateHandler(IFileInFolderCreateServices fileInFolderCreateServices)
        {
            _fileInFolderCreateServices = fileInFolderCreateServices;
            _fileInFolderCreateServices.NotNull(nameof(fileInFolderCreateServices));
        }

        public async Task Handle(string serverPath,string localFilePath)
        {
            await _fileInFolderCreateServices.Services(serverPath,localFilePath);
        }
    }
}
