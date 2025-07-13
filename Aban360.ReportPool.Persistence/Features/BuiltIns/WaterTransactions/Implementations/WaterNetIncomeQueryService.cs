using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class WaterNetIncomeQueryService : AbstractBaseConnection, IWaterNetIncomeQueryService
    {
        public WaterNetIncomeQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<WaterNetIncomeHeaderOutputDto, WaterNetIncomeDataOutputDto>> Get(WaterNetIncomeInputDto input)
        {
            string waterNetIncomeQueryString = GetWaterNetIncomeQuery();
            IEnumerable<WaterNetIncomeDataOutputDto> data = await _sqlReportConnection.QueryAsync<WaterNetIncomeDataOutputDto>(waterNetIncomeQueryString);//todo:Params
            WaterNetIncomeHeaderOutputDto header = new()
            { };

            ReportOutput<WaterNetIncomeHeaderOutputDto, WaterNetIncomeDataOutputDto> result = new(ReportLiterals.WaterNetIncome, header, data);
             return result;
        }
        private string GetWaterNetIncomeQuery()
        {
            return @"";
        }
    }
}
