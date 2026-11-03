namespace Aban360.UserPool.Domain.Features.SubscriberRequests;

public interface ISubscriberRequestStore
{
    Task<IReadOnlyList<SubscriberRequestDefinition>> GetDefinitions(CancellationToken cancellationToken);
    Task<ISubscriberRequestTransaction> Begin(Guid ownerId, CancellationToken cancellationToken);
}

public interface ISubscriberRequestTransaction : IAsyncDisposable
{
    Task<SubscriberRequestDefinition?> GetDefinition(SubscriberRequestCode code, CancellationToken ct);
    Task<SubscriberRequestDraft?> GetDraft(Guid id, Guid ownerId, CancellationToken ct);
    Task InsertDraft(SubscriberRequestDraft draft, CancellationToken ct);
    Task SaveDraft(SubscriberRequestDraft draft, CancellationToken ct);
    Task<SubscriberRequestChallenge?> GetLatestChallenge(Guid requestId, CancellationToken ct);
    Task InsertChallenge(SubscriberRequestChallenge challenge, CancellationToken ct);
    Task SaveChallenge(SubscriberRequestChallenge challenge, CancellationToken ct);
    Task LockMobile(string mobile, CancellationToken ct);
    Task<int> CountRecentChallenges(Guid ownerId, string mobile, DateTime since, CancellationToken ct);
    Task RegisterSubmission(SubscriberRequestDraft draft, CancellationToken ct);
    Task Commit(CancellationToken ct);
}
