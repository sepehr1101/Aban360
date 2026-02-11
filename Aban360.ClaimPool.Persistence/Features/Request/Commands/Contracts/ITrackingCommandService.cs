using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;

namespace Aban360.ClaimPool.Persistence.Features.Request.Commands.Contracts
{
    public interface ITrackingCommandService
    {
        Task Insert(TrackingInsertDto inputDto);
        Task UpdateIsConsiderdLatest(int trackNumber, bool isConsiderd);
    }
}
