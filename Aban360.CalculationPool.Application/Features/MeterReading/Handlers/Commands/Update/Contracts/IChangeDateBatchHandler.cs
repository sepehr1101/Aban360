using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.ApplicationUser;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Update.Contracts
{
    public interface IChangeDateBatchHandler
    {
        Task<ChangeDateBatchOutputDto> Handle(ChangeDateBatchInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }
}
