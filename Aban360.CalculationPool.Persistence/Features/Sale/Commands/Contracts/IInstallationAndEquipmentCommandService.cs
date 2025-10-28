using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;

namespace Aban360.CalculationPool.Persistence.Features.Sale.Commands.Contracts
{
    public interface IInstallationAndEquipmentCommandService
    {
        Task Create(InstallationAndEquipmentInputDto input);
        Task Update(InstallationAndEquipmentInputDto create, DeleteDto delete);
        Task Delete(DeleteDto input);
    }
}
