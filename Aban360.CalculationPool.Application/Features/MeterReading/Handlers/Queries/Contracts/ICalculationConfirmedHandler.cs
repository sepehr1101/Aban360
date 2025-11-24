using Aban360.Common.ApplicationUser;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts
{
    public interface ICalculationConfirmedHandler
    {
        Task Handle(int latestFlowId, IAppUser appUser, CancellationToken cancellationToken);
    }
}
