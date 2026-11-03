using Aban360.UserPool.Domain.Features.SubscriberRequests;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Aban360.UserPool.Persistence.Features.SubscriberRequests;

public sealed class SubscriberRequestStore(IConfiguration configuration) : ISubscriberRequestStore
{
    private SqlConnection Connection()
    {
        var configured = configuration.GetConnectionString("SubscriberRequestsConnection") ??
            configuration.GetConnectionString("DefaultConnection") ??
            throw new InvalidOperationException("Configure ConnectionStrings:SubscriberRequestsConnection for Atlas.");
        var builder = new SqlConnectionStringBuilder(configured) { InitialCatalog = "Atlas" };
        return new SqlConnection(builder.ConnectionString);
    }

    public async Task<IReadOnlyList<SubscriberRequestDefinition>> GetDefinitions(CancellationToken cancellationToken)
    {
        await using var connection = Connection();
        return (await connection.QueryAsync<SubscriberRequestDefinition>(new CommandDefinition("""
            SELECT d.* FROM SubscriberRequest.Definition d
            WHERE d.IsEnabled=1 AND d.Version=(SELECT MAX(n.Version) FROM SubscriberRequest.Definition n
                WHERE n.RequestCode=d.RequestCode) ORDER BY d.RequestCode;
            """, cancellationToken: cancellationToken))).AsList();
    }

    public async Task<ISubscriberRequestTransaction> Begin(Guid ownerId, CancellationToken cancellationToken)
    {
        var connection = Connection();
        try
        {
            await connection.OpenAsync(cancellationToken);
            var transaction = (SqlTransaction)await connection.BeginTransactionAsync(cancellationToken);
            var session = new SubscriberRequestTransaction(connection, transaction);
            try { await session.Lock("SubscriberRequest:owner:" + ownerId.ToString("N"), cancellationToken); }
            catch { await session.DisposeAsync(); throw; }
            return session;
        }
        catch { await connection.DisposeAsync(); throw; }
    }
}

internal sealed class SubscriberRequestTransaction(SqlConnection connection, SqlTransaction transaction) : ISubscriberRequestTransaction
{
    private CommandDefinition Command(string sql, object? parameters, CancellationToken ct) =>
        new(sql, parameters, transaction, commandTimeout: 30, cancellationToken: ct);

    internal async Task Lock(string resource, CancellationToken ct)
    {
        await connection.ExecuteAsync(Command("""
            DECLARE @result int;
            EXEC @result=sys.sp_getapplock @Resource=@resource, @LockMode='Exclusive',
                @LockOwner='Transaction', @LockTimeout=15000;
            IF @result < 0 THROW 51050, 'Subscriber request lock could not be acquired.', 1;
            """, new { resource }, ct));
    }

    public Task LockMobile(string mobile, CancellationToken ct) => Lock("SubscriberRequest:mobile:" + mobile, ct);

    public Task<SubscriberRequestDefinition?> GetDefinition(SubscriberRequestCode code, CancellationToken ct) =>
        connection.QuerySingleOrDefaultAsync<SubscriberRequestDefinition>(Command("""
            SELECT TOP(1) * FROM SubscriberRequest.Definition WHERE RequestCode=@code ORDER BY Version DESC;
            """, new { code = (long)code }, ct));

    public Task<SubscriberRequestDraft?> GetDraft(Guid id, Guid ownerId, CancellationToken ct) =>
        connection.QuerySingleOrDefaultAsync<SubscriberRequestDraft>(Command("""
            SELECT * FROM SubscriberRequest.Draft WITH (UPDLOCK) WHERE Id=@id AND OwnerId=@ownerId;
            """, new { id, ownerId }, ct));

    public Task InsertDraft(SubscriberRequestDraft draft, CancellationToken ct) => connection.ExecuteAsync(Command("""
        INSERT SubscriberRequest.Draft
            (Id,TrackingCode,OwnerId,BillId,Mobile,RequestCode,DefinitionVersion,HandlerName,PayloadSchema,Payload,Status,CreatedAt,FailedAttempts)
        VALUES (@Id,@TrackingCode,@OwnerId,@BillId,@Mobile,@RequestCode,@DefinitionVersion,@HandlerName,@PayloadSchema,@Payload,@Status,@CreatedAt,@FailedAttempts);
        """, DraftParameters(draft), ct));

    public Task SaveDraft(SubscriberRequestDraft draft, CancellationToken ct) => connection.ExecuteAsync(Command("""
        UPDATE SubscriberRequest.Draft SET Status=@Status,ConfirmedAt=@ConfirmedAt,CompletedAt=@CompletedAt,
            FailedAttempts=@FailedAttempts,ResultJson=@ResultJson WHERE Id=@Id AND OwnerId=@OwnerId;
        """, draft, ct));

    private static object DraftParameters(SubscriberRequestDraft draft) => new
    {
        draft.Id, draft.TrackingCode, draft.OwnerId, draft.BillId, draft.Mobile,
        RequestCode = (long)draft.RequestCode, draft.DefinitionVersion, draft.HandlerName,
        draft.PayloadSchema, draft.Payload, Status = (int)draft.Status, draft.CreatedAt, draft.FailedAttempts
    };

    public Task<SubscriberRequestChallenge?> GetLatestChallenge(Guid requestId, CancellationToken ct) =>
        connection.QuerySingleOrDefaultAsync<SubscriberRequestChallenge>(Command("""
            SELECT TOP(1) * FROM SubscriberRequest.Challenge WHERE RequestId=@requestId ORDER BY Sequence DESC;
            """, new { requestId }, ct));

    public Task InsertChallenge(SubscriberRequestChallenge challenge, CancellationToken ct) => connection.ExecuteAsync(Command("""
        INSERT SubscriberRequest.Challenge (Id,RequestId,CodeHash,CreatedAt,DeliveryFailed)
        VALUES (@Id,@RequestId,@CodeHash,@CreatedAt,@DeliveryFailed);
        """, challenge, ct));

    public Task SaveChallenge(SubscriberRequestChallenge challenge, CancellationToken ct) => connection.ExecuteAsync(Command("""
        UPDATE SubscriberRequest.Challenge SET SentAt=@SentAt,ExpiresAt=@ExpiresAt,ConsumedAt=@ConsumedAt,
            DeliveryFailed=@DeliveryFailed WHERE Id=@Id AND RequestId=@RequestId;
        """, challenge, ct));

    public Task<int> CountRecentChallenges(Guid ownerId, string mobile, DateTime since, CancellationToken ct) =>
        connection.ExecuteScalarAsync<int>(Command("""
            -- Owner/mobile application locks serialize relevant sends; skip other callers' uncommitted rows.
            SELECT COUNT(*) FROM SubscriberRequest.Challenge c WITH (READPAST,READCOMMITTEDLOCK)
            JOIN SubscriberRequest.Draft d WITH (READPAST,READCOMMITTEDLOCK) ON d.Id=c.RequestId
            WHERE c.CreatedAt>=@since AND (d.OwnerId=@ownerId OR d.Mobile=@mobile);
            """, new { ownerId, mobile, since }, ct));

    public Task RegisterSubmission(SubscriberRequestDraft draft, CancellationToken ct) => connection.ExecuteAsync(Command("""
        INSERT SubscriberRequest.Submission (RequestId,RequestCode,OwnerId,BillId,Payload,RegisteredAt)
        VALUES (@Id,@RequestCode,@OwnerId,@BillId,@Payload,SYSUTCDATETIME());
        """, new { draft.Id, RequestCode = (long)draft.RequestCode, draft.OwnerId, draft.BillId, draft.Payload }, ct));

    public Task Commit(CancellationToken ct) => transaction.CommitAsync(ct);

    public async ValueTask DisposeAsync()
    {
        await transaction.DisposeAsync();
        await connection.DisposeAsync();
    }
}
