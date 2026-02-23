using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Implementations
{
    internal sealed class TavizQueryService : AbstractBaseConnection, ITavizQueryService
    {
        public TavizQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        //public async Task<LatesTavizInfo> Get(ZoneIdAndCustomerNumberOutputDto input)
        //{
        //    string dbName = GetDbName(input.ZoneId);
        //    string query = GetQuery(dbName);
        //    LatesTavizInfo result = await _sqlReportConnection.QueryFirstOrDefaultAsync<LatesTavizInfo>(query, input);
        //    return result;
        //}
        public async Task<IEnumerable<MeterChangeInfoOutputDto>> Get(ZoneIdAndCustomerNumber input)
        {
            string dbName = GetDbName(input.ZoneId);
            string query = GetQuery(dbName);
            IEnumerable<MeterChangeInfoOutputDto> result = await _sqlReportConnection.QueryAsync<MeterChangeInfoOutputDto>(query, input);
            return result;
        }
        private string GetQuery(string dbName)
        {
            return $@"Select 
					radif CustomerNumber,
					taviz_no MeterNumber,
					taviz_date MeterChangeDateJalali,
					date_sabt RegisterDateJalali,
					TRIM(serial) BodySerial,
					elat ChangeCauseId,
					m.Title ChangeCauseTitle
				From [{dbName}].dbo.taviz t
				Join [Db70].dbo.MeterCause m
					ON t.elat=m.Id
				Where
					t.town=@zoneId AND
					t.radif=@customerNumber
				Order By t.taviz_date Desc,t.ID Desc";
        }

    }
}
