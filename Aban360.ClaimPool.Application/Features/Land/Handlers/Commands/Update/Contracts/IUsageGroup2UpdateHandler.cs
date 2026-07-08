using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.Common.ApplicationUser;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts
{
    public interface IUsageGroup2UpdateHandler
    {
        Task Handle(UsageGroup2UpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }
}
