using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;

namespace Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Delete.Contracts
{
    public interface IZoneDeleteHandler
    {
        Task Handle(ZoneDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
