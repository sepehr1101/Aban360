using System.Runtime.InteropServices;

namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Querys.Contracts
{
    public interface ICreateFolderHandler
    {
        Task<string> Handle(string folderName, CancellationToken cancellationToken, [Optional] string path );
    }
}
