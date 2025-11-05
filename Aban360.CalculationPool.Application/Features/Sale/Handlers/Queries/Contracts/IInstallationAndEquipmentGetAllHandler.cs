using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts
{
    public interface IInstallationAndEquipmentGetAllHandler
    {
        Task<IEnumerable<InstallationAndEquipmentOutputDto>> Handle(CancellationToken cancellationToken);
    }
}
