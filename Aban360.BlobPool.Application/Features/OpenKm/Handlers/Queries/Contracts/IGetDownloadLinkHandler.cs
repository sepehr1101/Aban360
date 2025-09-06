namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Contracts
{
    public interface IGetDownloadLinkHandler
    {
        Task<string> Handle(string uuid, bool oneTimeUse, CancellationToken cancellationToken);
    }
}
