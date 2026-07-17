using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.Common.ApplicationUser;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts
{
    public interface IMeterSmsFlowCreateHandler
    {
        Task Handle(MeterSmsFlowInsertInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }
}
