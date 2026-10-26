using Aban360.UserPool.Domain.Features.Ticketing.Entities;

namespace Aban360.UserPool.Persistence.Features.Ticketing.Commands.Contracts
{
    /// <summary>Stages changes; the caller commits through IUnitOfWork.</summary>
    public interface ITicketRatingCommandService
    {
        Task Add(TicketRating entity, CancellationToken cancellationToken = default);
        void Update(TicketRating entity);
    }
}
