using Aban360.Common.ApplicationUser;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Contracts
{
    public interface IKartableAccessUpdateHandler
    {
        Task Handle(KartableAccessUpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }
}
