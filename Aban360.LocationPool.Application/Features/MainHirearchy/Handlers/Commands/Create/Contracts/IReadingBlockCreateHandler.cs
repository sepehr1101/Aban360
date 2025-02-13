using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;

namespace Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Create.Contracts
{
    public interface IReadingBlockCreateHandler
    {
        Task Handle(ReadingBlockCreateDto createDto, CancellationToken cancellationToken);
    }
}
