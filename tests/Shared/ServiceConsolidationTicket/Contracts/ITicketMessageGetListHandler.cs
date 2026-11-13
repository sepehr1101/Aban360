using Aban360.Common.ApplicationUser;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;

namespace Aban360.UserPool.Application.Features.Ticketing.Handlers.Queries.Contracts
{
    public interface ITicketMessageGetListHandler
    {
        Task<TicketPage<TicketMessageGetDto>> Handle(TicketMessageSearchInput input, IAppUser currentUser, CancellationToken cancellationToken);
    }
}
