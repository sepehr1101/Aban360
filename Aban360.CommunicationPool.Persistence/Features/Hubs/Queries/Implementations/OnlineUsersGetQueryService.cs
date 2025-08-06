using Aban360.Common.Db.Dapper;
using Aban360.CommunicationPool.Domain.Features.Hubs.Dto.Queries;
using Aban360.CommunicationPool.Persistence.Features.Hubs.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.CommunicationPool.Persistence.Features.Hubs.Queries.Implementations
{

    internal sealed class OnlineUsersGetQueryService : AbstractBaseConnection, IOnlineUsersGetQueryService
    {
        public OnlineUsersGetQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<IEnumerable<OnlineUserGetDto>> Get()
        {
            string hubEventQueryService = GetServerReportsByIdQuery();
            IEnumerable<OnlineUserGetDto> data = await _sqlConnection.QueryAsync<OnlineUserGetDto>(hubEventQueryService);
            return data;
        }
        private string GetServerReportsByIdQuery()
        {
            return @"Select 
                        h.Id,
                    	h.UserId,
						u.FullName,
                    	h.ConnectDateTime,
                    	h.ConnectionId
                    From [Aban360].[CommunicationPool].HubEvent h
					Join [Aban360].[UserPool].[User] u on h.UserId=u.Id
                    Where 
                    h.DisconnectDateTime is null AND
                    h.ConnectDateTime > DATEADD(DAY, -1, GETDATE())";
        }
    }
}
