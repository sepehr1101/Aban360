using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts
{
    public interface IInstallationAndEquipmentUpdateHadler
    {
        Task Handle(InstallationAndEquipmentUpdateDto inputDto, CancellationToken cancellationToken);
    }
}
