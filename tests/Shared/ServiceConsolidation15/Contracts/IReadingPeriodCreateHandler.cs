// Test-only contract retained for pre-refactor snapshots.
using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;

namespace Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Create.Contracts
{
    public interface IReadingPeriodCreateHandler
    {
        Task Handle(ReadingPeriodCreateDto createDto, CancellationToken cancellationToken);
    }
}
