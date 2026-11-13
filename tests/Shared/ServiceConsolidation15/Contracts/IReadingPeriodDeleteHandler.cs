// Test-only contract retained for pre-refactor snapshots.
using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;

namespace Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Delete.Contracts
{
    public interface IReadingPeriodDeleteHandler
    {
        Task Handle(ReadingPeriodDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
