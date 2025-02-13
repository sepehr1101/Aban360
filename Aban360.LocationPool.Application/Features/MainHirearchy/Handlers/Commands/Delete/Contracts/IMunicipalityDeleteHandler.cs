using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;

namespace Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Delete.Contracts
{
    public interface IMunicipalityDeleteHandler
    {
        Task Handel(MunicipalityDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
