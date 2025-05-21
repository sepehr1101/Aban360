using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.Common.ApplicationUser;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts
{
    public interface IRequestSubscriptionCreateHandler
    {
        Task Handle(IAppUser currentUser, RequestSubscriptionCreateDto createDto, CancellationToken cancellationToken);
    }
}
