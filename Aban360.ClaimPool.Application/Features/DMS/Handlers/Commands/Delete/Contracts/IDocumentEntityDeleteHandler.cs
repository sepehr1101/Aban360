using Aban360.ClaimPool.Domain.Features.DMS.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.DMS.Handlers.Commands.Delete.Contracts
{
    public interface IDocumentEntityDeleteHandler
    {
        Task Handle(DocumentEntityDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
