using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Update.Contracts
{
    public interface IExecutableMimetypeUpdateHandler
    {
        Task Handle(ExecutableMimetypeUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
