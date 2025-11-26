using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.Common.ApplicationUser;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Update.Contracts
{
    public interface IMeterReadingDetailUpdateHandler
    {
        Task Handle(MeterReadingDetailUpdateDto input, IAppUser appUser, CancellationToken cancellationToken);
    }
}