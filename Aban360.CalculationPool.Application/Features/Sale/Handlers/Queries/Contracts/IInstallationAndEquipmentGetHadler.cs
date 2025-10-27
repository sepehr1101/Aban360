using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts
{
    public interface IInstallationAndEquipmentGetHadler
    {
        Task<InstallationAndEquipmentOutputDto> Handle(SearchById input, CancellationToken cancellationToken);
    }
}
