using Microsoft.AspNetCore.Http;

namespace Aban360.BlobPool.GatewayAddHoc.Features.Taxonomy.Contracts
{
    public interface IDocumentCommandAddhoc
    {
        Task<Guid> Handle(IFormFile file, string description, short documentTypeId, CancellationToken cancellationToken);
    }
}
