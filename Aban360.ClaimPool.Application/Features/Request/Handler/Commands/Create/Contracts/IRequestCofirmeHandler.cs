using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.Common.ApplicationUser;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts
{
    public interface IRequestCofirmeHandler
    {
        Task Handle(RequestConfirmInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }
}