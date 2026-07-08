using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.Common.ApplicationUser;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts
{
    public interface IUsageGroup3InsertHandler
    {
        Task Handle(UsageGroup3InsertDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }
}
