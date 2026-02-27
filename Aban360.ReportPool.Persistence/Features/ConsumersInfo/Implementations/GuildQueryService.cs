using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.ConsumersInfo.Implementations
{
    internal sealed class GuildQueryService : AbstractBaseConnection, IGuildQueryService
    {
        public GuildQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<NumericDictionary>> Get()
        {
            string query = GetQuery();
            IEnumerable<NumericDictionary> dictionary = await _sqlReportConnection.QueryAsync<NumericDictionary>(query);
            return dictionary;
        }
        private string GetQuery()
        {
            string query = @"USE Db70
                            SELECT MoshtarakinId Id, Title
                            FROM Senf";
            return query;
        }
    }
}
