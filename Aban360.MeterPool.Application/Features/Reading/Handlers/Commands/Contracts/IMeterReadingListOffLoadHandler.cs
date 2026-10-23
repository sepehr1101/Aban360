using Aban360.Common.ApplicationUser;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Commands;

namespace Aban360.MeterPool.Application.Features.Reading.Handlers.Commands.Contracts
{
    public interface IMeterReadingListOffLoadHandler
    {
        Task Handle(IEnumerable<MeterReadingListOffLoadInputDto> inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }
}
