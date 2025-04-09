using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Create.Contracts
{
    public interface IExecutableMimetypeCreateHandler
    {
        Task Handle(ExecutableMimetypeCreateDto createDto, CancellationToken cancellationToken);
    }
}
