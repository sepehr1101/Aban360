using Aban360.Common.ApplicationUser;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using Aban360.UserPool.Domain.Features.Ticketing.Entities;

namespace Aban360.UserPool.Application.Features.Ticketing.Services.Contracts
{
    public interface ITicketAccessService
    {
        Task<Ticket> Get(long ticketId, IAppUser currentUser, CancellationToken cancellationToken);
        void EnsureStaff(Ticket ticket, IAppUser currentUser);
        void EnsureSender(TicketMessage message, IAppUser currentUser);
        TicketSearchInput Scope(TicketSearchInput input, IAppUser currentUser);
    }
}
