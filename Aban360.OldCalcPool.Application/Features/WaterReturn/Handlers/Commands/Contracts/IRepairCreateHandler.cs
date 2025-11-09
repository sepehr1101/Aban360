using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands;

namespace Aban360.OldCalcPools.Application.Features.WaterReturn.Handlers.Commands.Contracts
{
    public interface IRepairCreateHandler
    {
        Task Handle(OfferingToCreateRepairDto createDto, CancellationToken cancellationToken);
    }
}
