using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aban360.ReportPool.Persistence.Features.ConsumersInfo.Implementations
{
    internal sealed class ReadingInBetweenService : AbstractBaseConnection, IReadingInBetweenService
    {
        public ReadingInBetweenService(IConfiguration configuration)
            : base(configuration)
        {

        }

        public async Task<IEnumerable<ReadingInBetweenOutput>> Get(ReadingInBetweenInput input)
        {
            string query = GetQuery(GetDbName(input.ZoneId));
            IEnumerable<ReadingInBetweenOutput> output = await _sqlReportConnection.QueryAsync<ReadingInBetweenOutput>(query, input);
            return output;
        }
        private string GetQuery(string dbName)
        {
            string query = $@"USE [{dbName}]
                            SELECT TOP 2000
                            town ZoneId,
                            radif CustomerNumber,
                            TRIM(eshtrak) ReadingNumber,
                            TRIM(bill_id) BillId
                            FROM members
                            WHERE 
	                            town=@zoneId AND
	                            trim(eshtrak) BETWEEN @fromReadingNumber AND @toReadingNumber";
            return query;
        }
    }
}
