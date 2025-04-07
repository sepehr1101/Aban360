using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Contracts
{
    public interface IRequestWaterMeterTagDeleteHandler
    {
        Task Handle(WaterMeterTagRequestDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
