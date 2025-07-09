using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class WaterRawSalesSummaryQueryService : AbstractBaseConnection, IWaterRawSalesSummaryQueryService
    {
        public WaterRawSalesSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<WaterRawSalesSummaryHeaderOutputDto, WaterRawSalesSummaryDataOutputDto>> GetInfo(WaterRawSalesSummaryInputDto input)
        {
            string waterRawSalesSummarys = GetWaterRawSalesSummaryQuery();
            IEnumerable<WaterRawSalesSummaryDataOutputDto> waterRawSalesData = await _sqlReportConnection.QueryAsync<WaterRawSalesSummaryDataOutputDto>(waterRawSalesSummarys);//todo: Parameters
            WaterRawSalesSummaryHeaderOutputDto waterRawSalesHeader = new WaterRawSalesSummaryHeaderOutputDto()
            { };

            var result = new ReportOutput<WaterRawSalesSummaryHeaderOutputDto, WaterRawSalesSummaryDataOutputDto>(ReportLiterals.WaterRawSalesSummary, waterRawSalesHeader, waterRawSalesData);
            return result;
        }

        private string GetWaterRawSalesSummaryQuery()
        {
            return @" ";
        }
    }
}
