using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Features.Usp.Input;
using Aban360.ReportPool.Domain.Features.Usp.Output;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ReportPool.Persistence.Features.Usp.Implementations
{
    public interface IUspPayment2QueryService
    {
        Task<IEnumerable<UspPayment2Output>> Get(UspPayment2Input input);
    }

    internal sealed class UspPayment2QueryService : AbstractBaseConnection, IUspPayment2QueryService
    {
        const string spName = "[GHABS].dbo.[usp_vosolab_01]";

        public UspPayment2QueryService(IConfiguration configuration) : base(configuration)
        {
        }
        public async Task<IEnumerable<UspPayment2Output>> Get(UspPayment2Input input)
        {
            DynamicParameters parameters = GetParams(input);
            IEnumerable<UspPayment2Output> output = await _sqlReportConnection.QueryAsync<UspPayment2Output>(spName, parameters, commandType: CommandType.StoredProcedure, commandTimeout: 360);
            return output;
        }
        private DynamicParameters GetParams(UspPayment2Input input)
        {
            DynamicParameters parameters = new();

            parameters.Add("@ABFAR_X", input.VillageOrCityType, DbType.Int32);
            parameters.Add("@Tar1", input.FromDateJalali, DbType.String);
            parameters.Add("@Tar2", input.ToDateJalali, DbType.String);
            parameters.Add("@C_bank1", input.FromBankCode, DbType.Int32);
            parameters.Add("@C_bank2", input.ToBankCode, DbType.Int32);
            parameters.Add("@noe_kar", input.UsageType, DbType.Int16);
            parameters.Add("@noe_report2", input.GroupingType, DbType.Int16);
            parameters.Add("@city", input.ZoneId, DbType.Int32);
            parameters.Add("@taj_sh", input.ZoneGroupingType, DbType.Int32);

            return parameters;
        }
    }
}
