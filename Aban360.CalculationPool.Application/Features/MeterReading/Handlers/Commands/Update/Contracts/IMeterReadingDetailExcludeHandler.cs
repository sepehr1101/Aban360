using Aban360.Common.ApplicationUser;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Update.Contracts
{
    public interface IMeterReadingDetailExcludeHandler
    {
        Task Handle(int id, IAppUser appUser, CancellationToken cancellationToken);
    }
}
