using Aban360.Common.ApplicationUser;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Ticketing.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;

namespace Aban360.UserPool.Application.Features.Ticketing.Handlers.Queries.Implementations
{
    internal sealed class TicketGetMyListHandler : ITicketGetMyListHandler
    {
        private readonly ITicketGetListHandler _ticketGetListHandler;
        public TicketGetMyListHandler(
            ITicketGetListHandler ticketGetListHandler)
        {
            _ticketGetListHandler = ticketGetListHandler;
            _ticketGetListHandler.NotNull(nameof(ticketGetListHandler));
        }

        public Task<TicketPage<TicketGetDto>> Handle(TicketMySearchInput input, IAppUser currentUser, CancellationToken cancellationToken)
        {
            input.NotNull(nameof(input));
            currentUser.NotNull(nameof(currentUser));
            // Always constrain ownership, including when the current user is an admin.
            // Reuse the existing search validation, query and DTO mapping.
            return _ticketGetListHandler.Handle(new TicketSearchInput
            {
                CreatedByUserId = currentUser.UserId,
                StatusId = input.StatusId,
                SearchText = input.SearchText,
                FromDate = input.FromDate,
                ToDateExclusive = input.ToDateExclusive,
                PageNumber = input.PageNumber,
                PageSize = input.PageSize
            }, currentUser, cancellationToken);
        }
    }
}
