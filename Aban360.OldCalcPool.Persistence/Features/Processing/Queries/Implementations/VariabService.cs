using Aban360.Common.Db.Dapper;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Implementations
{
    internal sealed class VariabService : AbstractBaseConnection, IVariabService
    {
        const string _atlasDbName = "Atlas";
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
        public async Task<decimal> GetAndRenewParvand(int zoneId, bool isAtlas)
        {
            string dbName = isAtlas ? _atlasDbName : GetDbName(zoneId);
            decimal parvand = await _sqlReportConnection.QueryFirstOrDefaultAsync<decimal>(GetParvandQuery(dbName, isAtlas), new { zoneId });
            await _sqlReportConnection.ExecuteAsync(IncreaseParavand(dbName, isAtlas), new { zoneId });
            return parvand;
        }
        public async Task<bool> IsOperationValid(int zoneId, string operationDate)
        {
            string dateCheck = await _sqlReportConnection.QuerySingleAsync<string>(GetCheckDate1(GetDbName(zoneId)), new { @zoneId });
            if (operationDate.CompareTo(dateCheck) < 0)
            {
                return false;
            }
            string _35daysAgo = DateTime.Now.AddDays(-35).ToShortPersianDateString();
            if (operationDate.CompareTo(_35daysAgo) < 0)
            {
                return false;
            }
            string today = DateTime.Now.ToShortPersianDateString();
            if (today.Substring(5, 2) != operationDate.Substring(5, 2))
            {
                //return false; //TODO: در صورتی که ماه متفاوت باشد اما دوره بسته نشده باشد موقتا گیر نده 1405/03/02 
            }
            return true;
        }
        public async Task<int> GetAndRenewRadif(int zoneId)
        {
            int radif = await _sqlReportConnection.QuerySingleAsync<int>(GetRadifQuery(GetDbName(zoneId)), new { zoneId });
            await _sqlReportConnection.ExecuteAsync(IncreaseRadif(GetDbName(zoneId)), new { zoneId });
            return radif;
        }
        public async Task<string> GetDateCheck(int zoneId)
        {
            string dbName = GetDbName(zoneId);
            string query = GetDateCheckQuery(dbName);
            string? dateCheck = await _sqlReportConnection.QueryFirstOrDefaultAsync<string>(query, new { zoneId });
            if (string.IsNullOrWhiteSpace(dateCheck))
            {
                throw new InvalidDataException(ExceptionLiterals.InvalidDateCheckFormat);
            }
            return dateCheck;
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

        private string GetRadifQuery(string dbName)
        {
            string query = $"USE [{dbName}] " +
                $"SELECT radif FROM variab Where town=@zoneId";
            return query;
        }
        private string IncreaseRadif(string dbName)
        {
            string query = $"USE [{dbName}] " +
                $"UPDATE variab SET radif=radif+1 Where town=@zoneId;";
            return query;
        }
        private string GetCheckDate1(string dbName)
        {
            string query = @$"USE [{dbName}]
                            SELECT date_check FROM variab Where town=@zoneId";
            return query;
        }

        private string GetParvandQuery(string dbName, bool isAtlas)
        {
            string zoneCondition = isAtlas ? string.Empty : " Where town=@zoneId";
            string query = $"USE [{dbName}] SELECT parvand FROM variab {zoneCondition} ;";
            return query;
        }
        private string IncreaseParavand(string dbName, bool isAtlas)
        {
            string zoneCondition = isAtlas ? string.Empty : " Where town=@zoneId";
            string query = $"USE [{dbName}] " +
               $"UPDATE variab SET parvand=parvand+1 {zoneCondition} ;";
            return query;
        }

        private string GetDateCheckQuery(string dbName)
        {
            return $@"Select date_check
                    From [{dbName}].dbo.variab 
                    Where town=@zoneId";
        }
    }
}
