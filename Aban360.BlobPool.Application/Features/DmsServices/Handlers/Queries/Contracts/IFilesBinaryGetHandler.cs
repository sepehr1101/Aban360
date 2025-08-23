using Aban260.BlobPool.Infrastructure.Features.DmsServices.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.BlobPool.Application.Features.DmsServices.Handlers.Queries.Contracts
{
    public interface IFilesBinaryGetHandler
    {
        Task<string> Handle(string documentId);
    }
    internal sealed class FilesBinaryGetHandler : IFilesBinaryGetHandler
    {
        private readonly IFileBinaryGetServices _fileBinaryGetSevice;
        public FilesBinaryGetHandler(IFileBinaryGetServices fileBinaryGetSevice)
        {
            _fileBinaryGetSevice = fileBinaryGetSevice;
            _fileBinaryGetSevice.NotNull(nameof(fileBinaryGetSevice));
        }

        public async Task<string> Handle(string documentId)
        {
            var result = await _fileBinaryGetSevice.Services(documentId);
            return result;
        }
    }
}
