using Aban360.ClaimPool.Domain.Features.DMS.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.DMS.Handlers.Commands.Create.Contracts
{
    public interface IDocumentEntityCreateHandler
    {
        Task Handle(DocumentEntityCreateDto createDto, CancellationToken cancellationToken);
    }
}
