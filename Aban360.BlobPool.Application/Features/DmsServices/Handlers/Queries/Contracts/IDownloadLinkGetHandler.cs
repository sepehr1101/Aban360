using Aban260.BlobPool.Infrastructure.Features.DmsServices.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.BlobPool.Application.Features.DmsServices.Handlers.Queries.Contracts
{
    public interface IDownloadLinkHandler
    {
        Task<string> Handle(string userId, bool onTimeUse);
    }
    internal sealed class DownloadLinkHandler : IDownloadLinkHandler
    {
        private readonly IDownloadLinkServices _downloadLinkSevice;
        public DownloadLinkHandler(IDownloadLinkServices downloadLinkSevice)
        {
            _downloadLinkSevice = downloadLinkSevice;
            _downloadLinkSevice.NotNull(nameof(downloadLinkSevice));
        }

        public async Task<string> Handle(string userId,bool onTimeUse)
        {
            return await _downloadLinkSevice.Services(userId,onTimeUse);
        }
    }
}
