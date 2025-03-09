using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;

namespace Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Create.Contracts
{
    public interface IReadingConfigDefaultCreateHandler
    {
        Task Handle(ReadingConfigDefaultCreateDto createDto, CancellationToken cancellationToken);
    }
}
