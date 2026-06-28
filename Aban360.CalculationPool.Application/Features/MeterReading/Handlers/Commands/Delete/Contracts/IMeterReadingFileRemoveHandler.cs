using Aban360.Common.ApplicationUser;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Delete.Contracts
{
    public interface IMeterReadingFileRemoveHandler
    {
        Task Handle(int id, IAppUser appUser, CancellationToken cancellation);
    }
}
