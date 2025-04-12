using Aban360.ClaimPool.Domain.Features.DMS.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.DMS.Handlers.Commands.Update.Contracts
{
    public interface IDocumentEntityUpdateHandler
    {
        Task Handle(DocumentEntityUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
