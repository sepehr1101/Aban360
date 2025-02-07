using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Create.Contracts
{
    public interface IHeadquarterCreateHandler
    {
        Task Handle(HeadquarterCreateDto createDto, CancellationToken cancellationToken);
    }
}
