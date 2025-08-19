using Aban260.BlobPool.Infrastructure.Features.DmsServices.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.BlobPool.Application.Features.DmsServices.Handlers.Queries.Contracts
{
    public interface IFilesGetHandler
    {
        Task<string> Handle(string fieldId);
    }
    internal sealed class FilesGetHandler : IFilesGetHandler
    {
        private readonly IFilesGetServices _getFilesSevice;
        public FilesGetHandler(IFilesGetServices getFilesSevice)
        {
            _getFilesSevice = getFilesSevice;
            _getFilesSevice.NotNull(nameof(getFilesSevice));
        }

        public async Task<string> Handle(string fieldId)
        {
            string result= await _getFilesSevice.Services(fieldId);
            return result;
        }
    }
}
