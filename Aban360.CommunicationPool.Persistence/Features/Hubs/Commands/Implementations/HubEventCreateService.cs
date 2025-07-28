using Aban360.Common.Db.Dapper;
using Aban360.CommunicationPool.Domain.Features.Hubs.Dto.Commands;
using Aban360.CommunicationPool.Persistence.Features.Hubs.Commands.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.CommunicationPool.Persistence.Features.Hubs.Commands.Implementations
{
    internal sealed class HubEventCreateService : AbstractBaseConnection, IHubEventCreateService
    {
        public HubEventCreateService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task Create(HubEventCreateDto input)
        {
            string createQuery = GetHubEventCreateQuery();
            var @params = new
            {
                connectionId = input.ConnectionId,
                userId = input.UserId,
                connectDateTime = DateTime.Now,
            };
            await _sqlConnection.ExecuteAsync(createQuery, @params);
        }

        private string GetHubEventCreateQuery()
        {
            return @"Insert Into [Aban360].CommunicationPool.HubEvent
                          (ConnectionId, UserId, ConnectDateTime)
                    Values(@connectionId,@userId,@connectDateTime)";
        }
    }
}
