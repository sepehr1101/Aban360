using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Delete.Contracts
{
    public interface IDocumentCategoryDeleteHandler
    {
        Task Handle(DocumentCategoryDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
