using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Update.Contracts
{
    public interface IReadingBlockUpdateHandler
    {
        Task Handle(ReadingBlockUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
