using global::Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using global::Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using global::Aban360.Common.BaseEntities;
using global::Aban360.Common.Extensions;
using global::Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using global::Aban360.UserPool.Domain.Features.Auth.Entities;
using global::Aban360.UserPool.Persistence.Constants.Enums;
using global::Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;

namespace BeforeServiceWorkflow10.Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Implementations{
    internal sealed class RequestKartableGetListByUserIdHandler : IRequestKartableGetListByUserIdHandler
    {
        private readonly IUserClaimQueryService _userClaimQueryService;
        private readonly IRequestStatusQueryService _requestStatusQueryService;
        public RequestKartableGetListByUserIdHandler(
            IUserClaimQueryService userClaimQueryService,
            IRequestStatusQueryService requestStatusQueryService)
        {
            _userClaimQueryService = userClaimQueryService;
            _userClaimQueryService.NotNull(nameof(userClaimQueryService));

            _requestStatusQueryService = requestStatusQueryService;
            _requestStatusQueryService.NotNull(nameof(requestStatusQueryService));
        }

        public async Task<IEnumerable<SelectionDto>> Handle(Guid userId, CancellationToken cancellationToken)
        {
            IEnumerable<SelectionDto> requestStatuses = await _requestStatusQueryService.GetIsKartable();
            ICollection<UserClaim> userAccesses = await _userClaimQueryService.GetValid(userId, ClaimType.RequestKartable);
            foreach (var item in requestStatuses)
            {
                item.IsSelected =userAccesses.Where(u => int.Parse(u.ClaimValue) == item.Id).Any();
                //userAccesses.Select(u=> int.Parse(u.ClaimValue)).Contains(item.Id);
            }

            return requestStatuses;
        }
    }
}