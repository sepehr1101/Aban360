using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Update.Contracts
{
    public interface IMimetypeCategoryUpdateHandler
    {
        Task Handle(MimetypeCategoryUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
