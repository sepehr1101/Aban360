using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Create.Contracts
{
    public interface IDocumentTypeCreateHandler
    {
        Task Handle(DocumentTypeCreateDto createDto, CancellationToken cancellationToken);
    }
}
