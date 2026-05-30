using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.ApplicationUser;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts
{
    public interface ICustomerUpdateHandler
    {
        Task Handle(SubscriptionGetDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
        Task Handle(CustomerEstateUpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
        Task Handle(CustomerTechnicalUpdateDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
        Task Handle(CustomerUpdate3Dto inputDto, IAppUser appUser, CancellationToken cancellationToken);
        Task Handle(CustomerUpdate5Dto inputDto, IAppUser appUser, CancellationToken cancellationToken);
        Task Handle(ServiceLinkConnectionInput inputDto, int deletionStateId, IAppUser appUser, CancellationToken cancellationToken);
        Task Handle(CustomerMobileUpdateInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
        Task Handle(CustomerBranchTypeUpdateInputDto inputDto, IAppUser appUser, CancellationToken cancellation);
    }
}
