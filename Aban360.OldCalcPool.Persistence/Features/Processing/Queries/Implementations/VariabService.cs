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
            decimal barge = await _sqlReportConnection.QuerySingleAsync<decimal>(GetBargeQuery(GetDbName(zoneId)), new { zoneId });
            await _sqlReportConnection.QuerySingleAsync<decimal>(GetBargeQuery(GetDbName(zoneId)), new { zoneId });
            await _sqlReportConnection.ExecuteAsync(IncreaseBarge(GetDbName(zoneId)), new { zoneId });
            return barge;
        }
        public async Task<decimal[]> GetAndRenew(int zoneId, int count)
        {
            decimal barge = await _sqlReportConnection.QuerySingleAsync<decimal>(GetBargeQuery(GetDbName(zoneId)), new { zoneId });
            await _sqlReportConnection.QuerySingleAsync<decimal>(GetBargeQuery(GetDbName(zoneId)), new { zoneId });
            await _sqlReportConnection.ExecuteAsync(IncreaseBarge(GetDbName(zoneId), count), new { zoneId });

            decimal[] range = new decimal[count];
            for (int i = 0; i < count; i++)
            {
                range[i] = barge + i;
            }

            return range;
        }

        public async Task<bool> IsOperationValid(int zoneId, string operationDate)
        {
            string dateCheck = await _sqlReportConnection.QuerySingleAsync<string>(GetCheckDate1(GetDbName(zoneId)), new { @zoneId });
            if (operationDate.CompareTo(dateCheck) < 0)
            {
                return false;
            }
            string _30daysAgo = DateTime.Now.AddDays(-30).ToShortPersianDateString();
            if (operationDate.CompareTo(_30daysAgo) < 0)
            {
                return false;
            }
            string today = DateTime.Now.ToShortPersianDateString();
            if (today.Substring(5, 2) != operationDate.Substring(5, 2))
            {
                return false;
            }
            return true;
        }

        private string GetBargeQuery(string dbName)
        {
            string query = $"USE [{dbName}] " +
                $"SELECT barge FROM variab Where town=@zoneId";
            return query;
        }
        private string IncreaseBarge(string dbName)
        {
            string query = $"USE [{dbName}] " +
                $"UPDATE variab SET barge=barge+1 Where town=@zoneId;";
            return query;
        }
        private string IncreaseBarge(string dbName, int count)
        {
            string query = $"USE [{dbName}] " +
                $"UPDATE variab SET barge=barge+{count}  Where town=@zoneId;";
            return query;
        }

        private string GetCheckDate1(string dbName)
        {
            string query = @$"USE [{dbName}]
                            SELECT date_check FROM variab Where town=@zoneId";
            return query;
        }
    }
}
