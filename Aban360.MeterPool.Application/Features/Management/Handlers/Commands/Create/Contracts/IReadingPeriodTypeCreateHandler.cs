using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;

namespace Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Create.Contracts
{
    public interface IReadingPeriodTypeCreateHandler
    {
        Task Handle(ReadingPeriodTypeCreateDto createDto, CancellationToken cancellationToken);
    }
}
