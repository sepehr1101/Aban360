using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;

namespace Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Create.Contracts
{
    public interface IMunicipalityCreateHandler
    {
        Task Handle(MunicipalityCreateDto createDto, CancellationToken cancellationToken);
    }
}
