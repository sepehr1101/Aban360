namespace Aban360.CommunicationPool.Persistence.Features.Hubs.Commands.Contracts
{
    public interface IHubPresenceService
    {
        Task Refresh(IReadOnlyCollection<string> connectionIds, CancellationToken cancellationToken);
    }
}
