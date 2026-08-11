using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.Common.ApplicationUser;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Update.Contracts
{
    public interface ISubscriptionAssignmentByTrackNumberUpdateHandler
    {
        Task Handle(SubscriptionAssignmentByTrackNumberUpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }
}
