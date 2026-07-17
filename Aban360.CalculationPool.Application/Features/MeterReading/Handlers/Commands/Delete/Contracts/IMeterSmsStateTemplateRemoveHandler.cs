using Aban360.Common.ApplicationUser;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Delete.Contracts
{
    public interface IMeterSmsStateTemplateRemoveHandler
    {
        Task Handle(short id, IAppUser appUser, CancellationToken cancellationToken);
    }
}
