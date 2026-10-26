using Aban360.Common.ApplicationUser;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands;

namespace Aban360.UserPool.Application.Features.Ticketing.Handlers.Commands.Create.Contracts
{
    public interface ITicketAttachmentCreateHandler
    {
        Task Handle(TicketAttachmentCreateDto inputDto, IAppUser currentUser, CancellationToken cancellationToken);
    }
}
