using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.Common.ApplicationUser;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Update.Contracts
{
    public interface ISwapRequestTypeHandler
    {
        Task Handle(SwapRequestTypeInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }
}
