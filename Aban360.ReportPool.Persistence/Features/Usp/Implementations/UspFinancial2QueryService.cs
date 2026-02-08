using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Features.Usp.Input;
using Aban360.ReportPool.Domain.Features.Usp.Output;
using Aban360.ReportPool.Persistence.Features.Usp.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ReportPool.Persistence.Features.Usp.Implementations
{
    internal sealed class UspFinancial2QueryService : AbstractBaseConnection, IUspFinancial2QueryService
    {
        const string spName = "[GHABS].dbo.usp_daramad2";
        const string spName2 = "[GHABS].dbo.usp_daramad2_2";

        public UspFinancial2QueryService(IConfiguration configuration) : base(configuration)
        {
        }
        public async Task<IEnumerable<UspFinancial2Output>> Get(UspFinancial2Input input)
        {
            string finalSpName=input.GroupingType<=2?spName:spName2;
            DynamicParameters parameters = GetParams(input);
            IEnumerable<UspFinancial2Output> output = await _sqlReportConnection.QueryAsync<UspFinancial2Output>(spName, parameters, commandType: CommandType.StoredProcedure);
            return output;
        }
        private DynamicParameters GetParams(UspFinancial2Input input)
        {
            DynamicParameters parameters = new();

            parameters.Add("@ABFAR_X", input.VillageOrCityType, DbType.Int32);
            parameters.Add("@Tar1", input.FromDateJalali, DbType.String);
            parameters.Add("@Tar2", input.ToDateJalali, DbType.String);
            parameters.Add("@noe_kar", input.UsageType, DbType.Int16);
            parameters.Add("@noe_report2", input.GroupingType, DbType.Int16);
            parameters.Add("@net_not_net", input.NetType, DbType.Int16);
            parameters.Add("@Azad_no", input.BranchGroupType, DbType.Int16);
            parameters.Add("@city", input.ZoneId, DbType.Int32);
            parameters.Add("@taj_sh", input.ZoneGroupingType, DbType.Int32);

            return parameters;
        }
    }
}
