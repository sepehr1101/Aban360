using Aban360.Common.ApplicationUser;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands;
using Aban360.UserPool.Domain.Features.Ticketing.Entities;

namespace Aban360.UserPool.Application.Features.Ticketing.Handlers.Commands.Create.Contracts
{
    public interface ITicketCreateHandler
    {
        // Returns the staged entity; the caller commits before reading the database-generated Id.
        Task<Ticket> Handle(TicketCreateDto inputDto, IAppUser currentUser, CancellationToken cancellationToken);
    }
}
