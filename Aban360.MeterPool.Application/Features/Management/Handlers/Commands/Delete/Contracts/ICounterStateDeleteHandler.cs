using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;

namespace Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Delete.Contracts
{
    public interface ICounterStateDeleteHandler
    {
        Task Handle(CounterStateDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
