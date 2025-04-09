using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Delete.Contracts
{
    public interface IDocumentDeleteHandler
    {
        Task Handle(DocumentDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
