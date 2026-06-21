using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.Common.ApplicationUser;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts
{
    public interface IConCompanyPersonnelRemoveHandler
    {
        Task Handle(ConCompanyPersonnelRemoveInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }
}
