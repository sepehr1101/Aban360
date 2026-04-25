using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Implementations
{
    internal sealed class HBedBesQueryService : AbstractBaseConnection, IHBedBesQueryService
    {
        public HBedBesQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<bool> Get(ZoneIdAndCustomerNumber inputDto)
        {
            string dbName = GetDbName(inputDto.ZoneId);
            string query = GetHasReturnedByCustomerNumberQuery(dbName);
            string fromDateJalali = DateTime.Now.AddYears(-4).ToShortPersianDateString();

            bool hasReturned = await _sqlReportConnection.QueryFirstOrDefaultAsync<bool>(query, new { inputDto.CustomerNumber, fromDateJalali });
          
            return hasReturned;
        }
        private string GetHasReturnedByCustomerNumberQuery(string dbName)
        {
            return $@"Select 1
                	From [{dbName}].dbo.Hbedbes
                	Where 
                		radif=@customerNumber AND 
                		date_bed>=@fromDateJalali";
        }
    }
}
