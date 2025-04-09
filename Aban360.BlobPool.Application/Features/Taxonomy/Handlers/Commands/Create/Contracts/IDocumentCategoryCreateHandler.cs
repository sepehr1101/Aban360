using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Create.Contracts
{
    public interface IDocumentCategoryCreateHandler
    {
        Task Handle(DocumentCategoryCreateDto createDto, CancellationToken cancellationToken);
    }
}
