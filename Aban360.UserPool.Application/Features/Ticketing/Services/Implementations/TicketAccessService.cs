using Aban360.Common.ApplicationUser;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Exceptions;
using Aban360.UserPool.Application.Features.Ticketing.Services.Contracts;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using Aban360.UserPool.Domain.Features.Ticketing.Entities;
using Aban360.UserPool.Persistence.Features.Ticketing.Queries.Contracts;

namespace Aban360.UserPool.Application.Features.Ticketing.Services.Implementations
{
    internal sealed class TicketAccessService : ITicketAccessService
    {
        private readonly ITicketQueryService _ticketQueryService;
        public TicketAccessService(
            ITicketQueryService ticketQueryService)
        {
            _ticketQueryService = ticketQueryService;
            _ticketQueryService.NotNull(nameof(ticketQueryService));
        }

        public async Task<Ticket> Get(long ticketId, IAppUser currentUser, CancellationToken cancellationToken)
        {
            EnsureAuthenticated(currentUser);
            var ticket = await _ticketQueryService.Get(ticketId, cancellationToken) ?? throw new InvalidIdException();
            if (!currentUser.IsAdmin && ticket.CreatedByUserId != currentUser.UserId && ticket.AssignedToUserId != currentUser.UserId)
                throw new InvalidIdException();
            return ticket;
        }

        public void EnsureStaff(Ticket ticket, IAppUser currentUser)
        {
            EnsureAuthenticated(currentUser);
            if (!currentUser.IsAdmin && ticket.AssignedToUserId != currentUser.UserId)
                throw new CustomValidationException("فقط مسئول تیکت یا مدیر مجاز به انجام این عملیات است.");
        }

        public void EnsureSender(TicketMessage message, IAppUser currentUser)
        {
            EnsureAuthenticated(currentUser);
            if (message.SenderUserId != currentUser.UserId)
                throw new CustomValidationException("فقط ارسال‌کننده پیام مجاز به ویرایش آن است.");
        }

        public TicketSearchInput Scope(TicketSearchInput input, IAppUser currentUser)
        {
            EnsureAuthenticated(currentUser);
            // Never mutate the caller's DTO or accept an arbitrary user's scope.
            return new TicketSearchInput
            {
                CreatedByUserId = currentUser.IsAdmin || input.AssignedToUserId == currentUser.UserId
                    ? input.CreatedByUserId : currentUser.UserId,
                AssignedToUserId = input.AssignedToUserId,
                StatusId = input.StatusId, SearchText = input.SearchText,
                FromDate = input.FromDate, ToDateExclusive = input.ToDateExclusive,
                PageNumber = input.PageNumber, PageSize = input.PageSize
            };
        }

        private static void EnsureAuthenticated(IAppUser currentUser)
        {
            currentUser.NotNull(nameof(currentUser));
            if (currentUser.UserId == Guid.Empty)
                throw new CustomValidationException("کاربر جاری معتبر نیست.");
        }
    }
}
