// Test-only contract for unchanged pre-refactor snapshots.
using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;

namespace Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Delete.Contracts
{
    public interface IReadingConfigDefaultDeleteHandler
    {
        Task Handle(ReadingConfigDefaultDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
