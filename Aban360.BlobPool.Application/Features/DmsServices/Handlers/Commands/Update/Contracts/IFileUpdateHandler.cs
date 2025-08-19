using Aban260.BlobPool.Infrastructure.Features.DmsServices.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.BlobPool.Application.Features.DmsServices.Handlers.Commands.Update.Contracts
{
    public interface IFileUpdateHandler
    {
        Task Handle(string nodeId, string groupName);
    }
    internal sealed class FileUpdateHandler : IFileUpdateHandler
    {
        private readonly IFileEditServices _fileEditServices;
        public FileUpdateHandler(IFileEditServices fileEditServices)
        {
            _fileEditServices = fileEditServices;
            _fileEditServices.NotNull(nameof(fileEditServices));
        }

        public async Task Handle(string nodeId,string groupName)
        {
            await _fileEditServices.Services(nodeId, groupName);
        }
    }
}
