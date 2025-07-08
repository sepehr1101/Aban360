using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class WaterNetSalesSummaryQueryService : AbstractBaseConnection, IWaterNetSalesSummaryQueryService
    {
        public WaterNetSalesSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<WaterNetSalesSummaryHeaderOutputDto, WaterNetSalesSummaryDataOutputDto>> GetInfo(WaterNetSalesSummaryInputDto input)
        {
            string waterNetSalesSummarys = GetWaterNetSalesSummaryQuery();
            IEnumerable<WaterNetSalesSummaryDataOutputDto> waterNetSalesData = await _sqlConnection.QueryAsync<WaterNetSalesSummaryDataOutputDto>(waterNetSalesSummarys);//todo: Parameters
            WaterNetSalesSummaryHeaderOutputDto waterNetSalesHeader = new WaterNetSalesSummaryHeaderOutputDto()
            { };

            var result = new ReportOutput<WaterNetSalesSummaryHeaderOutputDto, WaterNetSalesSummaryDataOutputDto>(ReportLiterals.WaterNetSalesSummary, waterNetSalesHeader, waterNetSalesData);
            return result;
        }

        private string GetWaterNetSalesSummaryQuery()
        {
            return @" ";
        }
    }
}
