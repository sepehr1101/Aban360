using Aban360.Common.Db.Dapper;
using Aban360.CommunicationPool.Domain.Features.Hubs.Dto.Commands;
using Aban360.CommunicationPool.Persistence.Features.Hubs.Commands.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.CommunicationPool.Persistence.Features.Hubs.Commands.Implementations
{
    internal sealed class HubEventUpdateService : AbstractBaseConnection, IHubEventUpdateService
    {
        public HubEventUpdateService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task CloseConnection(HubEventUpdateDto input)
        {
            string updateQuery = GetCloseConnectionQuery();
            var @params = new
            {
                connectionId = input.ConnectionId,
                disconnectDateTime = DateTime.Now,
            };
            await using var connection = _sqlConnection;
            await connection.ExecuteAsync(updateQuery, @params);
        }

        public async Task CloseAllConnection(HubCloseConnectionsDto hubCloseConnectionsDto)
        {
            string query= GetColseAllConnectionsQuery();
            var @params = new
            {
                userId = hubCloseConnectionsDto.UserId,
                disconnectDateTime = DateTime.Now,
            };
            await using var connection = _sqlConnection;
            await connection.ExecuteAsync(query, @params);
        }

        private string GetCloseConnectionQuery()
        {
            return @"Update [Aban360].CommunicationPool.HubEvent
                    Set DisconnectDateTime=@disconnectDateTime                  
                    Where ConnectionId=@connectionId AND DisconnectDateTime IS NULL";
        }
        private string GetColseAllConnectionsQuery()
        {
            return @"Update [Aban360].CommunicationPool.HubEvent
                    Set DisconnectDateTime=@disconnectDateTime                  
                    Where UserId=@userId AND DisconnectDateTime IS NULL";
        }
    }
}
