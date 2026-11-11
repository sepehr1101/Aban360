namespace Aban360.UserPool.Application.Features.Ticketing.Services.Contracts
{
    public interface ITicketNotificationService
    {
        // Call only after the ticket/message transaction has committed.
        Task Created(string ticketNumber);
        Task Answered(long ticketId, Guid senderUserId);
    }
}
