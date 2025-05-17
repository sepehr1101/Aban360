using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Microsoft.AspNetCore.Http;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Create.Contracts
{
    public interface IDocumentCreateHandler
    {
        Task<Guid> Handle(IFormFile documentFile, short documentTypeId, string? description, CancellationToken cancellationToken);
    }
}
