using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;

namespace Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts
{
    public interface IInstallationAndEquipmentQueryService
    {
        Task<InstallationAndEquipmentOutputDto> Get(InstallationAndEquipmentGetDto input);
        Task<InstallationAndEquipmentOutputDto> Get(int id, string currentDateJalali);
        Task<IEnumerable<InstallationAndEquipmentOutputDto>> Get(string currentDateJalali);
    }
}
