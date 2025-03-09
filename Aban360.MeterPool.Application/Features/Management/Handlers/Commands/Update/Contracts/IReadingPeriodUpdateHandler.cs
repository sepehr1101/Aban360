using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;

namespace Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Update.Contracts
{
    public interface IReadingPeriodUpdateHandler
    {
        Task Handle(ReadingPeriodUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
