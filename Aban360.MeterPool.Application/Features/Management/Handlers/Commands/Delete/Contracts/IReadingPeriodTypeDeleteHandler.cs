using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;

namespace Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Delete.Contracts
{
    public interface IReadingPeriodTypeDeleteHandler
    {
        Task Handle(ReadingPeriodTypeDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
