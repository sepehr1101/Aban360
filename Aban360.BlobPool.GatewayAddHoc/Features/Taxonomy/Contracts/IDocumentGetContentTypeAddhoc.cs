namespace Aban360.BlobPool.GatewayAddHoc.Features.Taxonomy.Contracts
{
    public interface IDocumentGetContentTypeAddhoc
    {
        Task<byte[]> Handle(Guid id, CancellationToken cancellationToken);
    }
}
