using Aban360.Common.Db.Dapper;
using Aban360.CommunicationPool.Persistence.Features.Hubs.Commands.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.CommunicationPool.Persistence.Features.Hubs.Commands.Implementations
{
    internal sealed class HubPresenceService : AbstractBaseConnection, IHubPresenceService
    {
        public HubPresenceService(IConfiguration configuration) : base(configuration) { }

        public async Task Refresh(IReadOnlyCollection<string> connectionIds, CancellationToken cancellationToken)
        {
            if (connectionIds.Count == 0) return;
            await using var connection = _sqlConnection;
            await connection.OpenAsync(cancellationToken);
            foreach (var batch in connectionIds.Chunk(500))
            {
                await connection.ExecuteAsync(new CommandDefinition(
                    """
                    UPDATE [Aban360].CommunicationPool.HubEvent
                    SET LastSeenDateTime = SYSDATETIME()
                    WHERE DisconnectDateTime IS NULL AND ConnectionId IN @ConnectionIds
                    """, new { ConnectionIds = batch }, cancellationToken: cancellationToken));
            }
        }
    }
}
