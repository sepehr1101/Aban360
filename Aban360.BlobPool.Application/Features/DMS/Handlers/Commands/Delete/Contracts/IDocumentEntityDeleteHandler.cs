using Aban360.BlobPool.Domain.Features.DMS.Dto.Commands;

namespace Aban360.BlobPool.Application.Features.DMS.Handlers.Commands.Delete.Contracts
{
    public interface IDocumentEntityDeleteHandler
    {
        Task Handle(DocumentEntityDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
