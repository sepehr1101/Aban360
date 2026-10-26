using Aban360.UserPool.Domain.Features.Ticketing.Entities;

namespace Aban360.UserPool.Persistence.Features.Ticketing.Commands.Contracts
{
    /// <summary>Stages changes; the caller commits through IUnitOfWork.</summary>
    public interface ITicketCommandService
    {
        Task Add(Ticket entity, CancellationToken cancellationToken = default);
        void Update(Ticket entity);
    }
}
