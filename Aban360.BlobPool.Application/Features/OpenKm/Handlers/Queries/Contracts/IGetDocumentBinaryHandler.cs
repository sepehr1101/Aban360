namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Contracts
{
    public interface IGetDocumentBinaryHandler
    {
        Task<byte[]> Handle(string documentId, bool isThumbnail, CancellationToken cancellationToken);
    }
}