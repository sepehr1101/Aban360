using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Delete.Contracts
{
    public interface IExecutableMimetypeDeleteHandler
    {
        Task Handle(ExecutableMimetypeDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
