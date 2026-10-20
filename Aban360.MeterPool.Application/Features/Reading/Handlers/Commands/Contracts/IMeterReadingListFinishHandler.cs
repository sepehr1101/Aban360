using Aban360.Common.ApplicationUser;

namespace Aban360.MeterPool.Application.Features.Reading.Handlers.Commands.Contracts
{
    public interface IMeterReadingListFinishHandler
    {
        Task Handle(long trackNumber, IAppUser appUser, CancellationToken cancellationToken);
    }
}
