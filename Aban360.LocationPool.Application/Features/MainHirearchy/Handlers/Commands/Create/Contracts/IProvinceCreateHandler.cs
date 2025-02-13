using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;

namespace Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Create.Contracts
{
    public interface IProvinceCreateHandler
    {
        Task Handle(ProvinceCreateDto createDto, CancellationToken cancellationToken);
    }
}
