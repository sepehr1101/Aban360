using Microsoft.AspNetCore.Http;

namespace Aban360.BlobPool.GatewayAddHoc.Features.Taxonomy.Contracts
{
    public interface IDocumentCommandAddhoc
    {
        Task Handle(IFormFile file, string description, CancellationToken cancellationToken);
    }
}
