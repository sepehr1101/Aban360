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

        public async Task Update(HubEventUpdateDto input)
        {
            string UpdateQuery = GetHubEventUpdateQuery();
            var @params = new
            {
                connectionId = input.ConnectionId,
                disconnectDateTime = DateTime.Now,
            };
            await _sqlConnection.ExecuteAsync(UpdateQuery, @params);
        }

        private string GetHubEventUpdateQuery()
        {
            return @"Update [Aban360].Communication.HubEvent
                    Set DisconnectDateTime=@disconnectDateTime                  
                    Where ConnectionId=@connectionId";
        }
    }
}