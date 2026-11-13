using Aban360.Common.ApplicationUser;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands;

namespace Aban360.UserPool.Application.Features.Ticketing.Handlers.Commands.Update.Contracts
{
    public interface ITicketAttachmentUpdateHandler
    {
        Task Handle(TicketAttachmentUpdateDto inputDto, IAppUser currentUser, CancellationToken cancellationToken);
    }
}
