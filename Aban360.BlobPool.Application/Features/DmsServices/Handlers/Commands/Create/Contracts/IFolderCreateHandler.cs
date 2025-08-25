using Aban260.BlobPool.Infrastructure.Features.DmsServices.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.BlobPool.Application.Features.DmsServices.Handlers.Commands.Create.Contracts
{
    public interface IFolderCreateHandler
    {
        Task<string> Handle(string folderPath);
    }
    internal sealed class FolderCreateHandler : IFolderCreateHandler
    {
        private readonly IFolderCreateServices _createFolderServices;
        public FolderCreateHandler(IFolderCreateServices createFolderServices)
        {
            _createFolderServices = createFolderServices;
            _createFolderServices.NotNull(nameof(createFolderServices));
        }

        public async Task<string> Handle(string folderPath)
        {
            string result = await _createFolderServices.Services(folderPath);
            return result;
        }
    }
}
