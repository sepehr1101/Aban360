using Aban360.Common.Db.Dapper;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Implementations
{
    internal sealed class VariabService : AbstractBaseConnection, IVariabService
    {
        public VariabService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<decimal> GetAndRenew(int zoneId)
        {            
            decimal barge = await _sqlReportConnection.QuerySingleAsync<decimal>(GetBargeQuery(GetDbName(zoneId)));
            await _sqlReportConnection.QuerySingleAsync<decimal>(GetBargeQuery(GetDbName(zoneId)));
            await _sqlReportConnection.ExecuteAsync(IncreaseBarge(GetDbName(zoneId)));
            return barge;
        }

        public async Task<bool> IsOperationValid(int zoneId, string operationDate)
        {
            string dateCheck = await _sqlReportConnection.QuerySingleAsync<string>(GetCheckDate1(GetDbName(zoneId)));
            if (operationDate.CompareTo(dateCheck) > 0)
            {
                return false;
            }
            string _15daysAgo = DateTime.Now.AddDays(-15).ToShortPersianDateString();
            if (operationDate.CompareTo(_15daysAgo) < 0)
            {
                return false;
            }
            return true;
        }

        private string GetBargeQuery(string dbName)
        {
            string query = $"USE [{dbName}] " +
                $"SELECT barge FROM variab";
            return query;
        }
        private string IncreaseBarge(string dbName)
        {
            string query = $"USE [{dbName}] " +
                $"UPDATE variab SET barge=barge+1;";
            return query;
        }
        
        private string GetCheckDate1(string dbName)
        {
            string query = @$"USE [{dbName}]
                            SELECT date_check FROM variab";
        }
    }
}
